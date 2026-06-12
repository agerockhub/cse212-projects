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

    // Test helper to support tree.Reverse().AsString() mapping syntax
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
    public static BinarySearchTree CreateTreeFromSortedList(int[] sortedList)
    {
        var tree = new BinarySearchTree();
        if (sortedList.Length > 0)
        {
            InsertMiddle(sortedList, 0, sortedList.Length - 1, tree);
        }
        return tree;
    }

    // Problem 5: Create Tree from Sorted List
    private static void InsertMiddle(int[] sortedList, int first, int last, BinarySearchTree tree)
    {
        if (first > last)
        {
            return;
        }

        // Midpoint selection without integer allocation overflows
        int middle = first + (last - first) / 2;

        tree.Insert(sortedList[middle]);

        // Binary partition splitting steps
        InsertMiddle(sortedList, first, middle - 1, tree);
        InsertMiddle(sortedList, middle + 1, last, tree);
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
        tree.Insert(7); // Duplicate check element entry
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
        Assert.IsTrue(tree.Contains(7));
        Assert.IsFalse(tree.Contains(9));
    }
}

[TestClass]
public class TreeReverseTests
{
    [TestMethod]
    public void TreeReverse_Basic()
    {
        BinarySearchTree tree = new();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(4);
        tree.Insert(10);
        tree.Insert(1);
        tree.Insert(6);

        Assert.AreEqual("<IEnumerable>{10, 7, 6, 5, 4, 3, 1}", string.Join(", ", tree.Reverse().AsString()));
    }
}

[TestClass]
public class TreeGetHeightTests
{
    [TestMethod]
    public void TreeGetHeight_Basic()
    {
        BinarySearchTree tree = new();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(7);
        tree.Insert(4);
        tree.Insert(10);
        tree.Insert(1);
        tree.Insert(6);
        Assert.AreEqual(3, tree.GetHeight());
        tree.Insert(6);
        Assert.AreEqual(3, tree.GetHeight());
        tree.Insert(12);
        Assert.AreEqual(4, tree.GetHeight());
    }
}

[TestClass]
public class CreateTreeFromSortedListTests
{
    [TestMethod]
    public void CreateTreeFromSortedList_CountBy10s()
    {
        var tree = Trees.CreateTreeFromSortedList([10, 20, 30, 40, 50, 60]);
        Assert.AreEqual("<Bst>{10, 20, 30, 40, 50, 60}", tree.ToString());
        Assert.AreEqual(3, tree.GetHeight());
    }

    [TestMethod]
    public void CreateTreeFromSortedList_127Nodes()
    {
        var tree = Trees.CreateTreeFromSortedList(Enumerable.Range(0, 127).ToArray());
        Assert.AreEqual("<Bst>{" + string.Join(", ", Enumerable.Range(0, 127)) + "}", tree.ToString());
        Assert.AreEqual(7, tree.GetHeight());
    }

    [TestMethod]
    public void CreateTreeFromSortedList_128Nodes()
    {
        var tree = Trees.CreateTreeFromSortedList(Enumerable.Range(0, 128).ToArray());
        Assert.AreEqual("<Bst>{" + string.Join(", ", Enumerable.Range(0, 128)) + "}", tree.ToString());
        Assert.AreEqual(8, tree.GetHeight());
    }

    [TestMethod]
    public void CreateTreeFromSortedList_Single()
    {
        var tree = Trees.CreateTreeFromSortedList([42]);
        Assert.AreEqual("<Bst>{42}", tree.ToString());
        Assert.AreEqual(1, tree.GetHeight());
    }

    [TestMethod]
    public void CreateTreeFromSortedList_Empty()
    {
        var tree = Trees.CreateTreeFromSortedList([]);
        Assert.AreEqual("<Bst>{}", tree.ToString());
        Assert.AreEqual(0, tree.GetHeight());
    }
}
