using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System;

internal static class StringHelper
{
    public static List<string> SplitIntoLines(this string input, int maxLineLength)
    {
        var result = new List<string>();
        var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var line = "";

        var i = 0;
        while (i < words.Length)
        {
            var word = words[i];

            while (word.Length > maxLineLength)
            {
                if (line.Length > 0)
                {
                    var spaceLeft = maxLineLength - line.Length - 1; // account for space
                    if (spaceLeft > 0)
                    {
                        line += " " + word[..spaceLeft];
                        result.Add(line);
                        word = word[spaceLeft..];
                    }
                    else
                    {
                        result.Add(line);
                    }

                    line = "";
                }
                else
                {
                    result.Add(word[..maxLineLength]);
                    word = word[maxLineLength..];
                }
            }

            if (line.Length + word.Length + (line.Length > 0 ? 1 : 0) <= maxLineLength)
            {
                if (line.Length > 0) line += " ";

                line += word;
            }
            else
            {
                if (line.Length > 0) result.Add(line);

                line = word;
            }

            i++;
        }

        if (line.Length > 0) result.Add(line);

        return result;
    }
}