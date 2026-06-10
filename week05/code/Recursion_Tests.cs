using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class Recursion
{
    /// <summary>
    /// Problem 1: Recursive Squares Sum
    /// Finds the sum of 1^2 + 2^2 + 3^2 + ... + n^2 using recursion.
    /// O(n) performance.
    /// </summary>
    public static int SumSquaresRecursive(int n)
    {
        // Base case: If n <= 0, return 0 per instructions
        if (n <= 0) return 0;

        // Recursive step
        return (n * n) + SumSquaresRecursive(n - 1);
    }

    /// <summary>
    /// Problem 2: Permutations Choose
    /// Returns permutations of length 'size' from a string of unique letters.
    /// </summary>
    public static void PermutationsChoose(List<string> results, string letters, int size, string current = "")
    {
        // Base case: successfully selected 'size' number of letters
        if (current.Length == size)
        {
            results.Add(current);
            return;
        }

        // Recursive case: iterate through available unique letters
        for (int i = 0; i < letters.Length; i++)
        {
            char choice = letters[i];

            // Re-create the remaining letters string excluding the chosen letter
            string remaining = letters.Substring(0, i) + letters.Substring(i + 1);

            // Recurse down with the letter appended
            PermutationsChoose(results, remaining, size, current + choice);
        }
    }

    /// <summary>
    /// Problem 3: Climbing Stairs (with Memoization)
    /// Counts ways to climb s stairs taking 1, 2, or 3 steps at a time.
    /// O(s) performance due to memoization cache tracking.
    /// </summary>
    public static long CountWaysToClimb(int s, Dictionary<int, long> remember = null)
    {
        // Initialize memoization cache dictionary if it's the first call
        if (remember == null)
        {
            remember = new Dictionary<int, long>();
        }

        // Base cases
        if (s < 0) return 0;  // Invalid path
        if (s == 0) return 1; // Valid ground level reached successfully

        // Check if value was already computed and memoized
        if (remember.ContainsKey(s))
        {
            return remember[s];
        }

        // Recursive combination calculation formula
        long ways = CountWaysToClimb(s - 1, remember) +
                    CountWaysToClimb(s - 2, remember) +
                    CountWaysToClimb(s - 3, remember);

        // Store result in cache before returning
        remember[s] = ways;
        return ways;
    }

    /// <summary>
    /// Problem 4: Wildcard Binary Patterns
    /// Recursively substitutes all wildcard '*' elements with '0' and '1'.
    /// </summary>
    public static void WildcardBinaryPatterns(List<string> results, string pattern)
    {
        int wildcardIndex = pattern.IndexOf('*');

        // Base case: No wildcards left, add finished binary string to results
        if (wildcardIndex == -1)
        {
            results.Add(pattern);
            return;
        }

        // Split pattern around the first wildcard position
        string before = pattern.Substring(0, wildcardIndex);
        string after = pattern.Substring(wildcardIndex + 1);

        // Branch 1: Substitute wildcard with '0'
        WildcardBinaryPatterns(results, before + "0" + after);

        // Branch 2: Substitute wildcard with '1'
        WildcardBinaryPatterns(results, before + "1" + after);
    }

    /// <summary>
    /// Problem 5: Maze Solver
    /// Uses recursion to explore the n x n square grid and find all viable paths to the end.
    /// </summary>
    public static void SolveMaze(List<string> results, int[] maze, int n, int x, int y, List<(int, int)> currPath)
    {
        // 1. Add current grid coordinate to path tracker list
        currPath.Add((x, y));

        // 2. Base Case: If this is the ending target cell, format and save path
        if (IsEnd(maze, n, x, y))
        {
            results.Add(currPath.AsString());
        }
        else
        {
            // 3. Recursive Cases: Check all 4 directional movement possibilities
            // Directions array mappings: Up (0, -1), Down (0, 1), Left (-1, 0), Right (1, 0)
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nextX = x + dx[i];
                int nextY = y + dy[i];

                // Use the template's built-in validation rules tracker
                if (IsValidMove(maze, n, nextX, nextY, currPath))
                {
                    SolveMaze(results, maze, n, nextX, nextY, currPath);
                }
            }
        }

        // 4. Backtrack step: remove current position from path tracking before winding back up
        currPath.RemoveAt(currPath.Count - 1);
    }

    // Stub placeholders for the validation helpers already provided inside your project files:
    private static bool IsEnd(int[] maze, int n, int x, int y)
    {
        // This is handled by the instructor's hidden implementation code
        return maze[y * n + x] == 2;
    }

    private static bool IsValidMove(int[] maze, int n, int x, int y, List<(int, int)> currPath)
    {
        // This is handled by the instructor's hidden implementation code
        if (x < 0 || x >= n || y < 0 || y >= n) return false;
        if (maze[y * n + x] == 0) return false;
        if (currPath.Contains((x, y))) return false;
        return true;
    }
}


/// <summary>
/// Support extensions safely providing formatting outputs to match assignment results hooks.
/// </summary>
public static class PathExtensions
{
    public static string AsString(this List<(int, int)> path)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < path.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append($"({path[i].Item1}, {path[i].Item2})");
        }
        sb.Append("]");
        return sb.ToString();
    }
}
