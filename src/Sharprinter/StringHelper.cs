using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
///     Provides string extension methods for text manipulation and formatting.
/// </summary>
public static class StringHelper
{
    /// <summary>
    ///     Wraps the text to fit within the specified maximum line length by breaking lines at word boundaries.
    /// </summary>
    /// <param name="text">The text to wrap.</param>
    /// <param name="maxLineLength">The maximum number of characters per line.</param>
    /// <returns>A string with line breaks inserted to ensure no line exceeds the maximum length.</returns>
    public static string Wrap(this string text, int maxLineLength)
    {
        if (string.IsNullOrEmpty(text)) return text;

        var lines = text.Replace("\r\n", "\n").Split('\n');
        var wrappedLines = new List<string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                wrappedLines.Add(line);
                continue;
            }

            var wrapped = line.SplitIntoLines(maxLineLength);
            wrappedLines.AddRange(wrapped);
        }

        return string.Join("\n", wrappedLines);
    }

    /// <summary>
    ///     Splits the input string into multiple lines, ensuring each line does not exceed the specified maximum length.
    ///     Words are kept intact and moved to the next line if they would exceed the maximum length.
    /// </summary>
    /// <param name="input">The string to split into lines.</param>
    /// <param name="maxLineLength">The maximum number of characters per line.</param>
    /// <returns>A list of strings, each representing a line that fits within the maximum length.</returns>
    private static List<string> SplitIntoLines(this string input, int maxLineLength)
    {
        var result = new List<string>();
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var line = "";

        foreach (var word in words)
        {
            if (line.Length + word.Length + (line.Length > 0 ? 1 : 0) <= maxLineLength)
            {
                if (line.Length > 0)
                {
                    line += " ";
                }

                line += word;
            }
            else
            {
                result.Add(line);
                line = word;
            }
        }

        if (line.Length > 0)
        {
            result.Add(line);
        }

        return result;
    }
}