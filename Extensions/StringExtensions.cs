using System.Text.RegularExpressions;

namespace TheTruthHurtsMe.Extensions;

public static class StringExtensions
{
    public static string AddLineBreaks(this string text, int maxLineLength = 80)
    {
        // Step 1: Add new lines after punctuation marks (periods, exclamation marks, and question marks)
        string pattern = @"([.!?])\s+";
        string replacedText = Regex.Replace(text, pattern, "$1\n");

        // Step 2: Break lines that exceed the maxLineLength without cutting words
        string[] words = replacedText.Split(' ');
        var result = new System.Text.StringBuilder();
        int currentLineLength = 0;

        foreach (string word in words)
        {
            // If adding the next word would exceed the line length, add a newline
            if (currentLineLength + word.Length + 1 > maxLineLength)
            {
                result.Append("\n");
                currentLineLength = 0;
            }

            result.Append(word + " ");
            currentLineLength += word.Length + 1;
        }

        return result.ToString().Trim();
    }
}