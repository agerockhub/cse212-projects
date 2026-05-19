using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QueuesAssignment
{
    // =========================================================================
    // PROBLEM 1: TAKING TURNS QUEUE - IMPLEMENTATION
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
        // Using a standard List but treating it strictly as a FIFO Queue
        private readonly List<Person> _people = new List<Person>();

        public int Length => _people.Count;

        /// <summary>
        /// Fix: Enqueue appends to the end of the collection to preserve FIFO order.
        /// </summary>
        public void AddPerson(string name, int turns)
        {
            var person = new Person(name, turns);
            _people.Add(person); // Appends to the back/end
        }

        /// <summary>
        /// Dequeues from the front of the collection and manages turn cycles.
        /// </summary>
        public Person GetNextPerson()
        {
            if (_people.Count == 0)
            {
                throw new InvalidOperationException("No one in the queue.");
            }

            // Remove from the front (index 0) to preserve true FIFO behavior
            var person = _people[0];
            _people.RemoveAt(0);

            // If the person has infinite turns (0 or less), re-add to the back
            if (person.Turns <= 0)
            {
                _people.Add(person);
            }
            else
            {
                person.Turns--;
                if (person.Turns > 0)
                {
                    _people.Add(person);
                }
            }

            return person;
        }
    }

    // =========================================================================
    // PROBLEM 1: TAKING TURNS QUEUE - TESTS
    // =========================================================================

    [TestClass]
    public class TakingTurnsQueueTests
    {
        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3) and
        // run until the queue is empty
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
        // Defect(s) Found: Expected:<Bob>. Actual:<Sue>. Queue order was reversed because Enqueue() added items to the front instead of the back. Fix: Make enqueue append to the end (FIFO order).
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
        // Defect(s) Found: Queue order was reversed during midway insertions because items were pushed to the front instead of the back. Fix: Correct internal indexing to append elements to the end.
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
        // Defect(s) Found: Expected:<Tim>. Actual:<Sue>. Same queue-order issue affected infinite-turn tests. Fix: Correct Enqueue() ordering.
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

            var infinitePerson = players.GetNextPerson();
            Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Tim (Forever), Sue (3)
        // Run 10 times.
        // Expected Result: Tim, Sue, Tim, Sue, Tim, Sue, Tim, Tim, Tim, Tim
        // Defect(s) Found: Same queue-order issue affected infinite-turn tests with negative values where front-loading items corrupted the sequence structure. Fix: Correct Enqueue() ordering.
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

            var infinitePerson = players.GetNextPerson();
            Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
        }

        [TestMethod]
        // Scenario: Try to get the next person from an empty queue
        // Expected Result: Exception should be thrown with appropriate error message.
        // Defect(s) Found: System thrown errors were unhandled, missing custom wrapper verification. Fix: Ensure precise check for empty collections to safely handle boundary operations.
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

    // =========================================================================
    // PROBLEM 2: PRIORITY QUEUE - IMPLEMENTATION
    // =========================================================================

    public class PriorityItem
    {
        public string Value { get; set; }
        public int Priority { get; set; }

        public PriorityItem(string value, int priority)
        {
            Value = value;
            Priority = priority;
        }
    }

    public class PriorityQueue
    {
        private readonly List<PriorityItem> _queue = new List<PriorityItem>();

        public int Length => _queue.Count;

        public void Enqueue(string value, int priority)
        {
            var item = new PriorityItem(value, priority);
            _queue.Add(item);
        }

        /// <summary>
        /// Fixes: Search entire queue, remove the highest-priority item properly.
        /// Use > instead of >= when comparing priorities to keep FIFO order for equal priorities.
        /// </summary>
        public string Dequeue()
        {
            if (_queue.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            int highestPriorityIndex = 0;

            // Search entire queue
            for (int i = 1; i < _queue.Count; i++)
            {
                // Fix: Use > instead of >= when comparing priorities to preserve equal priority FIFO order
                if (_queue[i].Priority > _queue[highestPriorityIndex].Priority)
                {
                    highestPriorityIndex = i;
                }
            }

            var item = _queue[highestPriorityIndex];
            _queue.RemoveAt(highestPriorityIndex);
            
            return item.Value;
        }
    }

    // =========================================================================
    // PROBLEM 2: PRIORITY QUEUE - TESTS
    // =========================================================================

    // Fix: Class and method names match standard patterns with explicit [TestClass] and [TestMethod] attributes so they are detected.
    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        // Scenario: Enqueue distinct priorities "A" (5), "B" (10), "C" (3) and verify sorting extraction.
        // Expected Result: "B", "A", "C"
        // Defect(s) Found: Expected:<A>. Actual:<B>. PriorityQueue.Dequeue() was not correctly selecting/removing the highest-priority item. Fix: Search entire queue, remove the highest-priority item properly.
        public void TestPriorityQueue_1_HighestPrioritySelection()
        {
            var priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue("A", 5);
            priorityQueue.Enqueue("B", 10);
            priorityQueue.Enqueue("C", 3);

            Assert.AreEqual("B", priorityQueue.Dequeue());
            Assert.AreEqual("A", priorityQueue.Dequeue());
            Assert.AreEqual("C", priorityQueue.Dequeue());
        }

        [TestMethod]
        // Scenario: Enqueue multiple items with equal maximum priorities to check arrival ordering stability.
        // Expected Result: First added item is extracted first.
        // Defect(s) Found: Equal priority ordering failure. FIFO order was not preserved for equal priorities. Fix: Use > instead of >= when comparing priorities.
        public void TestPriorityQueue_2_EqualPriorityFIFO()
        {
            var priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue("FirstHigh", 10);
            priorityQueue.Enqueue("SecondHigh", 10);

            Assert.AreEqual("FirstHigh", priorityQueue.Dequeue());
            Assert.AreEqual("SecondHigh", priorityQueue.Dequeue());
        }

        [TestMethod]
        // Scenario: Call Dequeue on a completely empty PriorityQueue instance.
        // Expected Result: System.InvalidOperationException with message "The queue is empty."
        // Defect(s) Found: System.InvalidOperationException: The queue is empty. Empty queue exception was not handled in the student test. Fix: Use try/catch and verify the exception message.
        public void TestPriorityQueue_3_EmptyQueueHandling()
        {
            var priorityQueue = new PriorityQueue();

            try
            {
                priorityQueue.Dequeue();
                Assert.Fail("An exception should have been thrown for an empty queue.");
            }
            catch (InvalidOperationException e)
            {
                // Fix: Verify the specific exception message string matches expectations
                Assert.AreEqual("The queue is empty.", e.Message);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
    }
}
