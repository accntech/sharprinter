using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System;

internal static class StringHelper
{
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

    public static List<string> SplitIntoLines(this string input, int maxLineLength)
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