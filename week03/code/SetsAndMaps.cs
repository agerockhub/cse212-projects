using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// Problem 1: Find symmetric pairs using an O(n) set approach.
    /// </summary>
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<string>();
        var results = new List<string>();

        foreach (var word in words)
        {
            // Reverse the 2-character word safely using character array positions
            string reversed = $"{word[1]}{word[0]}";

            // Special case: if letters are identical (e.g., "aa"), skip it per instructions
            if (word == reversed) continue;

            if (seen.Contains(reversed))
            {
                results.Add($"{reversed} & {word}");
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
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            // Ensure the line contains at least 4 columns (Index 3 is the 4th column)
            if (fields.Length >= 4)
            {
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
        var counts = new Dictionary<char, int>();

        // Process first word: ignore spaces, normalize to lowercase
        foreach (char c in word1.ToLower())
        {
            if (char.IsWhiteSpace(c)) continue;
            counts[c] = counts.GetValueOrDefault(c, 0) + 1;
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
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        var summary = new List<string>();
        if (featureCollection?.Features != null)
        {
            foreach (var feature in featureCollection.Features)
            {
                if (feature.Properties != null)
                {
                    summary.Add($"{feature.Properties.Place} - Mag {feature.Properties.Mag}");
                }
            }
        }

        return summary.ToArray();
    }
}

// ============================================================================
// SUPPORTING DATA MODELS FOR PROBLEM 5 DESERIALIZATION
// ============================================================================

public class FeatureCollection
{
    public List<Feature>? Features { get; set; }
}

public class Feature
{
    public EarthquakeProperties? Properties { get; set; }
}

public class EarthquakeProperties
{
    public string Place { get; set; } = string.Empty;
    public double? Mag { get; set; } // Nullable handles potential missing values safely
}
