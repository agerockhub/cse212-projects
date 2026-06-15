using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ============================================================================
// 1. NODE IMPLEMENTATION (Problems 1, 2, & 4)
// ============================================================================
public class Node
{
    public int Data { get; set; }
    public Node? Left { get; set; }
    public Node? Right { get; set; }

    public Node(int data)
    {
        this.Data = data;
    }

    // Problem 1: Insert Unique Values Only
    public void Insert(int value)
    {
        // If the value already exists, do nothing (ignore duplicates)
        if (value == Data)
        {
            return;
        }

        if (value < Data)
        {
            if (Left is null)
                Left = new Node(value);
            else
                Left.Insert(value);
        }
        else
        {
            if (Right is null)
                Right = new Node(value);
            else
                Right.Insert(value);
        }
    }

    // Problem 2: Contains
    public bool Contains(int value)
    {
        if (value == Data)
        {
            return true;
        }

        if (value < Data)
        {
            return Left != null && Left.Contains(value);
        }
        else
        {
            return Right != null && Right.Contains(value);
        }
    }

    // Problem 4: Tree Height
    public int GetHeight()
    {
        int leftHeight = Left?.GetHeight() ?? 0;
        int rightHeight = Right?.GetHeight() ?? 0;

        return 1 + Math.Max(leftHeight, rightHeight);
    }
}

// ============================================================================
// 2. BINARY SEARCH TREE IMPLEMENTATION (Problem 3)
// ============================================================================
public class BinarySearchTree
{
    private Node? _root;

    public void Insert(int value)
    {
        if (_root is null)
            _root = new Node(value);
        else
            _root.Insert(value);
    }

    public bool Contains(int value)
    {
        return _root != null && _root.Contains(value);
    }

    public int GetHeight()
    {
        return _root?.GetHeight() ?? 0;
    }

    public override string ToString()
    {
        return "<Bst>{" + string.Join(", ", TraverseForward()) + "}";
    }

    public IEnumerable<int> TraverseForward()
    {
        return TraverseForward(_root);
    }

    private IEnumerable<int> TraverseForward(Node? node)
    {
        if (node is not null)
        {
            foreach (var left in TraverseForward(node.Left)) yield return left;
            yield return node.Data;
            foreach (var right in TraverseForward(node.Right)) yield return right;
        }
    }

    // Maps tree.Reverse().AsString() framework test methods
    public IEnumerable<int> Reverse()
    {
        return TraverseBackward();
    }

    // Problem 3: Traverse Backwards
    public IEnumerable<int> TraverseBackward()
    {
        return TraverseBackward(_root);
    }

    private IEnumerable<int> TraverseBackward(Node? node)
    {
        if (node is not null)
        {
            // Reverse In-Order Traversal: Right -> Root -> Left
            foreach (var right in TraverseBackward(node.Right)) yield return right;
            yield return node.Data;
            foreach (var left in TraverseBackward(node.Left)) yield return left;
        }
    }
}

// Extender helper class to fulfill your explicit assertion syntax requirements
public static class EnumerableExtensions
{
    public static IEnumerable<int> AsString(this IEnumerable<int> source)
    {
        return source;
    }
}

// ============================================================================
// 3. TREE BALANCING HELPER IMPLEMENTATION (Problem 5)
// ============================================================================
public static class Trees
{
    /// <summary>
    /// Given a sorted list (sorted_list), create a balanced BST.
    /// </summary>
    public static BinarySearchTree CreateTreeFromSortedList(int[] sortedNumbers)
    {
        var bst = new BinarySearchTree();
        InsertMiddle(sortedNumbers, 0, sortedNumbers.Length - 1, bst);
        return bst;
    }

    private static void InsertMiddle(int[] sortedNumbers, int first, int last, BinarySearchTree bst)
    {
        // Base case: If pointers cross over, the segment range is empty.
        if (first > last)
        {
            return;
        }

        // Calculate the middle index without integer overflow issues.
        int middle = first + (last - first) / 2;

        // Insert the middle value into the tree.
        bst.Insert(sortedNumbers[middle]);

        // Recursively insert values from the left sub-array partition.
        InsertMiddle(sortedNumbers, first, middle - 1, bst);

        // Recursively insert values from the right sub-array partition.
        InsertMiddle(sortedNumbers, middle + 1, last, bst);
    }
}

// ============================================================================
// 4. UNIT TEST SUITE (Supplied Project Assertions)
// ============================================================================
[TestClass]
public class TreeInsertTests
{
    [TestMethod]
    public void TreeInsert_Basic()
    {
        BinarySearchTree tree = new();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(7); // Problem 1 verification element
        tree.Insert(4);
        tree.Insert(10);
        tree.Insert(1);
        tree.Insert(6);
        Assert.AreEqual("<Bst>{1, 3, 4, 5, 6, 7, 10}", tree.ToString());
    }
}

[TestClass]
public class TreeContainsTests
{
    [TestMethod]
    public void TreeContains_Basic()
    {
        BinarySearchTree tree = new();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(4);
        tree.Insert(10);
        tree.Insert(1);
        tree.Insert(6);

        Assert.IsTrue(tree.Contains(3));
        Assert.IsFalse(tree.Contains(2));
        Assert.IsTrue(tree.Contains(6));
        Assert.IsTrue(tree.Contains(7)); // Completed the cut-off statement safely
    }
}
