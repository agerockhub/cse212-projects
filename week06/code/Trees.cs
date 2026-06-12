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
    /// Given a sorted list (sorted_list), create a balanced BST.  If the values in the
    /// sortedNumbers were inserted in order from left to right into the BST, then it
    /// would resemble a linked list (unbalanced). To get a balanced BST, the
    /// InsertMiddle function is called to find the middle item in the list to add
    /// first to the BST. The InsertMiddle function takes the whole list but also takes
    /// a range (first to last) to consider.  For the first call, the full range of 0 to
    /// Length-1 used.
    /// </summary>
    public static BinarySearchTree CreateTreeFromSortedList(int[] sortedNumbers)
    {
        var bst = new BinarySearchTree(); // Create an empty BST to start with 
        InsertMiddle(sortedNumbers, 0, sortedNumbers.Length - 1, bst);
        return bst;
    }

    /// <summary>
    /// This function will attempt to insert the item in the middle of 'sortedNumbers' into
    /// the 'bst' tree. The middle is determined by using indices represented by 'first' and
    /// 'last'.
    /// For example, if the function was called on:
    ///
    /// sortedNumbers = new[]{10, 20, 30, 40, 50, 60};
    /// first = 0;
    /// last = 5;
    /// 
    /// then the value 30 (index 2 which is the middle) would be added 
    /// to the 'bst' (the insert function in the <see cref="BinarySearchTree"/> can be used
    /// to do this).   
    ///
    /// Subsequent recursive calls are made to insert the middle from the values 
    /// before 30 and the values after 30.  If done correctly, the order
    /// in which values are added (which results in a balanced bst) will be:
    /// 
    /// 30, 10, 20, 50, 40, 60
    /// 
    /// This function is intended to be called the first time by CreateTreeFromSortedList.
    ///
    /// The purpose for having the first and last parameters is so that we do 
    /// not need to create new sub-lists when we make recursive calls.  Avoid 
    /// using list slicing to create sub-lists to solve this problem.    
    /// </summary>
    /// <param name="sortedNumbers">input numbers that are already sorted</param>
    /// <param name="first">the first index in the sortedNumbers to insert</param>
    /// <param name="last">the last index in the sortedNumbers to insert</param>
    /// <param name="bst">the BinarySearchTree in which to insert the values</param>
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
        var tree = Trees.CreateTreeFromSortedList(new int[] { 10, 20, 30, 40, 50, 60 });
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
