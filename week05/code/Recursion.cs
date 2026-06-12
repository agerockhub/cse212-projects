using System;
using System.Collections;
using System.Collections.Generic;

public static class Recursion
{
    /// <summary>
    /// #############
    /// # Problem 1 #
    /// #############
    /// Using recursion, find the sum of 1^2 + 2^2 + 3^2 + ... + n^2
    /// and return it.
    /// Time Complexity: O(n)
    /// Space Complexity: O(n) due to the call stack.
    /// </summary>
    public static int SumSquaresRecursive(int n)
    {
        // Base case: if n is less than or equal to 0, return 0
        if (n <= 0)
        {
            return 0;
        }

        // Recursive step: n^2 + sum of squares of (n-1)
        return (n * n) + SumSquaresRecursive(n - 1);
    }

    /// <summary>
    /// #############
    /// # Problem 2 #
    /// #############
    /// Using recursion, insert permutations of length 'size' 
    /// from a list of 'letters' into the results list.
    /// Time Complexity: O(P(N, K)) where N = letters.Length and K = size.
    /// </summary>
    public static void PermutationsChoose(List<string> results, string letters, int size, string word = "")
    {
        // Base case: if the built word reaches the target size, save it
        if (word.Length == size)
        {
            results.Add(word);
            return;
        }

        // Recursive step: try every available letter for the current position
        for (int i = 0; i < letters.Length; i++)
        {
            char chosen = letters[i];
            // Remove the chosen letter from the pool for subsequent choices
            string remainingLetters = letters.Remove(i, 1);

            PermutationsChoose(results, remainingLetters, size, word + chosen);
        }
    }

    /// <summary>
    /// #############
    /// # Problem 3 #
    /// #############
    /// Counts ways to climb stairs using Memoization to prevent 
    /// exponential time complexity.
    /// Time Complexity: O(s) - Each step configuration is calculated exactly once.
    /// Space Complexity: O(s) - For storing values in the memoization dictionary.
    /// </summary>
    public static decimal CountWaysToClimb(int s, Dictionary<int, decimal>? remember = null)
    {
        // Initialize the memoization dictionary if it is null
        if (remember == null)
        {
            remember = new Dictionary<int, decimal>();
        }

        // Base Cases
        if (s <= 0) return 0;
        if (s == 1) return 1;
        if (s == 2) return 2;
        if (s == 3) return 4;

        // Check if the solution has already been computed
        if (remember.ContainsKey(s))
        {
            return remember[s];
        }

        // Solve using recursion with the tracking dictionary passed along
        decimal ways = CountWaysToClimb(s - 1, remember) +
                       CountWaysToClimb(s - 2, remember) +
                       CountWaysToClimb(s - 3, remember);

        // Store the result before returning
        remember[s] = ways;
        return ways;
    }

    /// <summary>
    /// #############
    /// # Problem 4 #
    /// #############
    /// Finds all binary string combinations by resolving the '*' wildcard.
    /// Time Complexity: O(2^M) where M is the number of wildcards.
    /// </summary>
    public static void WildcardBinary(string pattern, List<string> results)
    {
        // Find the index of the first wildcard character
        int index = pattern.IndexOf('*');

        // Base case: if no wildcard is found, the string is fully resolved
        if (index == -1)
        {
            results.Add(pattern);
            return;
        }

        // Split the pattern around the wildcard
        string prefix = pattern[..index];
        string suffix = pattern[(index + 1)..];

        // Recurse by replacing the wildcard with '0' and '1'
        WildcardBinary(prefix + "0" + suffix, results);
        WildcardBinary(prefix + "1" + suffix, results);
    }

    /// <summary>
    /// #############
    /// # Problem 5 #
    /// #############
    /// Backtracking maze solver to find paths from (0,0) to the destination.
    /// </summary>
    public static void SolveMaze(List<string> results, Maze maze, int x = 0, int y = 0, List<ValueTuple<int, int>>? currPath = null)
    {
        if (currPath == null)
        {
            currPath = new List<ValueTuple<int, int>>();
        }

        // Base check: avoid infinite loops/cycles by making sure we haven't visited this cell
        if (currPath.Contains((x, y)))
        {
            return;
        }

        // Boundary/Wall Check: Ensure position is valid to walk on
        if (!maze.IsValid(x, y))
        {
            return;
        }

        // Backtracking step: Add the current coordinate to the path
        currPath.Add((x, y));

        // Base Case: Destination reached
        if (maze.IsEnd(x, y))
        {
            results.Add(currPath.AsString());
        }
        else
        {
            // Explore all 4 adjacent directions recursively
            SolveMaze(results, maze, x + 1, y, currPath); // Right
            SolveMaze(results, maze, x - 1, y, currPath); // Left
            SolveMaze(results, maze, x, y + 1, currPath); // Down
            SolveMaze(results, maze, x, y - 1, currPath); // Up
        }

        // Remove the cell from the path when backtracking up the call stack
        currPath.RemoveAt(currPath.Count - 1);
    }
}

// Mock interface representation of the Maze class assumed by the project structure
public interface Maze
{
    bool IsValid(int x, int y);
    bool IsEnd(int x, int y);
}

public static class PathExtensions
{
    public static string AsString(this List<ValueTuple<int, int>> path)
    {
        return string.Join("->", path);
    }
}
