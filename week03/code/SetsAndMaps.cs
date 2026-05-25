using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// Problem 1: Find symmetric pairs using an O(n) set approach.
    /// </summary>
    public static string[] FindPairs(string[] words)
    {
        HashSet<string> seen = new HashSet<string>();
        List<string> results = new List<string>();

        foreach (string word in words)
        {
            if (word == null || word.Length < 2) continue;

            // Explicitly reverse the two characters using C# array notation
            string reversed = "" + word[1] + word[0];

            // Special case: if letters are identical (e.g., "aa"), skip it per instructions
            if (word == reversed) continue;

            if (seen.Contains(reversed))
            {
                results.Add(reversed + " & " + word);
            }
            else
            {
                seen.Add(word);
            }
        }

        return results.ToArray();
    }

    /// <summary>
    /// Problem 2: Summarize degrees from the 4th column of a text file.
    /// </summary>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        Dictionary<string, int> degrees = new Dictionary<string, int>();
        foreach (string line in File.ReadLines(filename))
        {
            string[] fields = line.Split(',');
            // Ensure the line contains at least 4 columns (Index 3 is the 4th column)
            if (fields.Length >= 4)
            {
                // Safely clean whitespace manually to avoid any extension compatibility problems
                string degree = fields[3].Trim();
                if (degrees.ContainsKey(degree))
                {
                    degrees[degree]++;
                }
                else
                {
                    degrees[degree] = 1;
                }
            }
        }

        return degrees;
    }

    /// <summary>
    /// Problem 3: Check if two words are anagrams using a character-count dictionary.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        Dictionary<char, int> counts = new Dictionary<char, int>();

        // Process first word: ignore spaces, normalize to lowercase
        foreach (char c in word1.ToLower())
        {
            if (char.IsWhiteSpace(c)) continue;

            if (counts.ContainsKey(c))
            {
                counts[c]++;
            }
            else
            {
                counts[c] = 1;
            }
        }

        // Process second word: subtract from counts
        foreach (char c in word2.ToLower())
        {
            if (char.IsWhiteSpace(c)) continue;
            if (!counts.ContainsKey(c)) return false;

            counts[c]--;
            if (counts[c] == 0) counts.Remove(c);
        }

        // If the dictionary is completely empty, they are exact anagrams
        return counts.Count == 0;
    }

    /// <summary>
    /// Problem 5: Fetch USGS JSON, parse into classes, and extract summary.
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://usgs.gov";
        using HttpClient client = new HttpClient();
        using HttpRequestMessage getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using Stream jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using StreamReader reader = new StreamReader(jsonStream);
        string json = reader.ReadToEnd();
        JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        // Uses the FeatureCollection model defined inside FeatureCollection.cs
        FeatureCollection featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        List<string> summary = new List<string>();
        if (featureCollection != null && featureCollection.Features != null)
        {
            foreach (Feature feature in featureCollection.Features)
            {
                if (feature != null && feature.Properties != null)
                {
                    summary.Add(feature.Properties.Place + " - Mag " + feature.Properties.Mag);
                }
            }
        }

        return summary.ToArray();
    }
}
