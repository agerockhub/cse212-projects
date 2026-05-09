using System;
using System.Collections.Generic;

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number'
    /// followed by multiples of 'number'.
    /// Example: MultiplesOf(7, 5) => {7, 14, 21, 28, 35}
    /// </summary>
    public static double[] MultiplesOf(double number, int length)
    {
        // PLAN:
        // 1. Create an array of size 'length'
        // 2. Loop from i = 0 to length - 1
        // 3. For each index, compute number * (i + 1)
        // 4. Store result in array
        // 5. Return array

        double[] result = new double[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = number * (i + 1);
        }

        return result;
    }

    /// <summary>
    /// Rotate the 'data' list to the right by 'amount'.
    /// Example:
    /// {1,2,3,4,5,6,7,8,9}, amount = 3
    /// => {7,8,9,1,2,3,4,5,6}
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // PLAN:
        // 1. Get size of list
        // 2. Normalize amount using modulo
        // 3. Take last 'amount' elements
        // 4. Take first part of list
        // 5. Clear original list
        // 6. Add last part first, then first part

        int size = data.Count;

        // Ensure rotation is within bounds
        amount = amount % size;

        // Get last part of list
        List<int> lastPart = data.GetRange(size - amount, amount);

        // Get first part of list
        List<int> firstPart = data.GetRange(0, size - amount);

        // Rebuild list
        data.Clear();
        data.AddRange(lastPart);
        data.AddRange(firstPart);
    }
}