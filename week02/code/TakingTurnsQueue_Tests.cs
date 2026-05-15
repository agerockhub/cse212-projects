using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TakingTurnsTestsAndImplementation
{
    // =========================================================================
    // CODE UNDER TEST (PRODUCTION CODE)
    // =========================================================================

    public class Person
    {
        public string Name { get; set; }
        public int Turns { get; set; }

        public Person(string name, int turns)
        {
            Name = name;
            Turns = turns;
        }
    }

    public class TakingTurnsQueue
    {
        private readonly Queue<Person> _queue = new Queue<Person>();

        public int Length => _queue.Count;

        public void AddPerson(string name, int turns)
        {
            var person = new Person(name, turns);
            _queue.Enqueue(person);
        }

        public Person GetNextPerson()
        {
            if (_queue.Count == 0)
            {
                throw new InvalidOperationException("No one in the queue.");
            }

            var person = _queue.Dequeue();

            // A value of 0 or less means that they have an infinite number of turns
            if (person.Turns <= 0)
            {
                _queue.Enqueue(person);
            }
            else
            {
                person.Turns--;
                if (person.Turns > 0)
                {
                    _queue.Enqueue(person);
                }
            }

            return person;
        }
    }

    // =========================================================================
    // UNIT TEST SUITE WITH DEFECT DOCUMENTATION
    // =========================================================================

    [TestClass]
    public class TakingTurnsQueueTests
    {
        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3) and
        // run until the queue is empty
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
        // Defect(s) Found: The queue behaves as a Stack (Last-In, First-Out) instead of a Queue (First-In, First-Out). 
        // Bob is added first but Sue is processed first. Additionally, players are completely removed from the 
        // structure after one turn regardless of remaining turns, causing the loop to terminate prematurely.
        public void TestTakingTurnsQueue_FiniteRepetition()
        {
            var bob = new Person("Bob", 2);
            var tim = new Person("Tim", 5);
            var sue = new Person("Sue", 3);

            Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, sue, tim, tim];

            var players = new TakingTurnsQueue();
            players.AddPerson(bob.Name, bob.Turns);
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            int i = 0;
            while (players.Length > 0)
            {
                if (i >= expectedResult.Length)
                {
                    Assert.Fail("Queue should have ran out of items by now.");
                }

                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
                i++;
            }
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3)
        // After running 5 times, add George with 3 turns.  Run until the queue is empty.
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, George, Sue, Tim, George, Tim, George
        // Defect(s) Found: LIFO/Stack structure defect cuts the sequence short. George is placed incorrectly 
        // in the collection processing sequence due to the inversion of data access order. Players are not 
        // re-queued after taking a turn.
        public void TestTakingTurnsQueue_AddPlayerMidway()
        {
            var bob = new Person("Bob", 2);
            var tim = new Person("Tim", 5);
            var sue = new Person("Sue", 3);
            var george = new Person("George", 3);

            Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, george, sue, tim, george, tim, george];

            var players = new TakingTurnsQueue();
            players.AddPerson(bob.Name, bob.Turns);
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            int i = 0;
            for (; i < 5; i++)
            {
                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
            }

            players.AddPerson("George", 3);

            while (players.Length > 0)
            {
                if (i >= expectedResult.Length)
                {
                    Assert.Fail("Queue should have ran out of items by now.");
                }

                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);

                i++;
            }
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (Forever), Sue (3)
        // Run 10 times.
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
        // Defect(s) Found: People with infinite turns (0 turns) are not added back to the back of the queue. 
        // They are permanently dropped from rotation after their very first turn.
        public void TestTakingTurnsQueue_ForeverZero()
        {
            var timTurns = 0;

            var bob = new Person("Bob", 2);
            var tim = new Person("Tim", timTurns);
            var sue = new Person("Sue", 3);

            Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, sue, tim, tim];

            var players = new TakingTurnsQueue();
            players.AddPerson(bob.Name, bob.Turns);
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            for (int i = 0; i < 10; i++)
            {
                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
            }

            // Verify that the people with infinite turns really do have infinite turns.
            var infinitePerson = players.GetNextPerson();
            Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Tim (Forever), Sue (3)
        // Run 10 times.
        // Expected Result: Tim, Sue, Tim, Sue, Tim, Sue, Tim, Tim, Tim, Tim
        // Defect(s) Found: People with infinite turns using negative values (-3) are dropped after one turn 
        // because the internal logic fails to recognize negative numbers as infinite turns.
        public void TestTakingTurnsQueue_ForeverNegative()
        {
            var timTurns = -3;
            var tim = new Person("Tim", timTurns);
            var sue = new Person("Sue", 3);

            Person[] expectedResult = [tim, sue, tim, sue, tim, sue, tim, tim, tim, tim];

            var players = new TakingTurnsQueue();
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            for (int i = 0; i < 10; i++)
            {
                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
            }

            // Verify that the people with infinite turns really do have infinite turns.
            var infinitePerson = players.GetNextPerson();
            Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
        }

        [TestMethod]
        // Scenario: Try to get the next person from an empty queue
        // Expected Result: Exception should be thrown with appropriate error message.
        // Defect(s) Found: Calling GetNextPerson on an empty data structure throws an unhandled 
        // ArgumentOutOfRangeException or InvalidOperationException with a default system message, 
        // instead of throwing an InvalidOperationException with the specific message "No one in the queue.".
        public void TestTakingTurnsQueue_Empty()
        {
            var players = new TakingTurnsQueue();

            try
            {
                players.GetNextPerson();
                Assert.Fail("Exception should have been thrown.");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("No one in the queue.", e.Message);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception e)
            {
                Assert.Fail(
                     string.Format("Unexpected exception of type {0} caught: {1}",
                                    e.GetType(), e.Message)
                );
            }
        }
    }
}
