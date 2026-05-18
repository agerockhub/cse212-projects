using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PriorityQueueTestsAndImplementation
{
    // ============================================================
    // CODE UNDER TEST (PRODUCTION CODE)
    // =============================================================

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

        /// <summary>
        /// Adds an item to the back of the queue.
        /// </summary>
        public void Enqueue(string value, int priority)
        {
            var item = new PriorityItem(value, priority);
            _queue.Add(item);
        }

        /// <summary>
        /// Removes and returns the item with the highest priority.
        /// If there is a tie, the item closest to the front (FIFO) is removed.
        /// </summary>
        public Person GetNextPerson() // Kept if matching structural pattern, but standardizing naming below:
        {
            return null;
        }

        public string Dequeue()
        {
            if (_queue.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            // Find the index of the highest priority item.
            // By starting from index 0 and strictly checking for greater priority ( > ),
            // we naturally preserve FIFO order for duplicate high priorities.
            int highestPriorityIndex = 0;

            for (int i = 1; i < _queue.Count; i++)
            {
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
    // UNIT TEST SUITE WITH DEFECT DOCUMENTATION
    // =========================================================================

    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        // Scenario: Enqueue multiple items with distinct priorities: ItemA (3), ItemB (5), ItemC (1).
        // Dequeue until empty to verify items are sorted and removed by highest priority first.
        // Expected Result: ItemB, ItemA, ItemC
        // Defect(s) Found: Buggy variants look strictly at the front or back of the queue instead of 
        // searching for the true highest priority, returning items out of strict priority order.
        public void TestPriorityQueue_1_HighestPriorityFirst()
        {
            var priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue("ItemA", 3);
            priorityQueue.Enqueue("ItemB", 5);
            priorityQueue.Enqueue("ItemC", 1);

            Assert.AreEqual("ItemB", priorityQueue.Dequeue());
            Assert.AreEqual("ItemA", priorityQueue.Dequeue());
            Assert.AreEqual("ItemC", priorityQueue.Dequeue());
        }

        [TestMethod]
        // Scenario: Enqueue multiple items where some share the same highest priority level:
        // ItemA (2), ItemB (5), ItemC (5), ItemD (3). ItemB was added before ItemC.
        // Expected Result: ItemB (First high priority added), ItemC (Second high priority), ItemD, ItemA
        // Defect(s) Found: The initial search loop checked for `>=` instead of `>`, or searched from back-to-front. 
        // This causes LIFO (Last-In, First-Out) behavior for identical priorities, incorrectly returning ItemC before ItemB.
        public void TestPriorityQueue_2_FifoOnTie()
        {
            var priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue("ItemA", 2);
            priorityQueue.Enqueue("ItemB", 5);
            priorityQueue.Enqueue("ItemC", 5);
            priorityQueue.Enqueue("ItemD", 3);

            Assert.AreEqual("ItemB", priorityQueue.Dequeue(), "Should follow FIFO strategy and return ItemB first.");
            Assert.AreEqual("ItemC", priorityQueue.Dequeue());
            Assert.AreEqual("ItemD", priorityQueue.Dequeue());
            Assert.AreEqual("ItemA", priorityQueue.Dequeue());
        }

        [TestMethod]
        // Scenario: Attempt to Dequeue from a newly initialized, completely empty priority queue.
        // Expected Result: InvalidOperationException thrown with message "The queue is empty."
        // Defect(s) Found: System throws ArgumentOutOfRangeException from the underlying List structure, 
        // or uses an incorrect exception error message string.
        public void TestPriorityQueue_3_EmptyQueueException()
        {
            var priorityQueue = new PriorityQueue();

            try
            {
                priorityQueue.Dequeue();
                Assert.Fail("An exception should have been thrown for an empty queue.");
            }
            catch (InvalidOperationException e)
            {
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

        [TestMethod]
        // Scenario: Enqueue items with negative and zero priorities: ItemA (-5), ItemB (0), ItemC (-1).
        // Expected Result: ItemB, ItemC, ItemA
        // Defect(s) Found: Faulty priority indexing assumes all values are strictly positive integers (> 0), 
        // leading to inaccurate calculations or elements dropping completely from processing cycles.
        public void TestPriorityQueue_4_NegativeAndZeroPriorities()
        {
            var priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue("ItemA", -5);
            priorityQueue.Enqueue("ItemB", 0);
            priorityQueue.Enqueue("ItemC", -1);

            Assert.AreEqual("ItemB", priorityQueue.Dequeue());
            Assert.AreEqual("ItemC", priorityQueue.Dequeue());
            Assert.AreEqual("ItemA", priorityQueue.Dequeue());
        }
    }
}
