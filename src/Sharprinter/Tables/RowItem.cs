using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Represents a row item in a table, providing methods for text wrapping, alignment, and length control.
/// </summary>
public interface IRowItem
{
    /// <summary>
    ///     Enables text wrapping for the row item.
    /// </summary>
    /// <returns>The current <see cref="IRowItem" /> instance.</returns>
    IRowItem TextWrap();

    /// <summary>
    ///     Sets the horizontal alignment for the row item.
    /// </summary>
    /// <param name="alignment">The desired horizontal alignment.</param>
    /// <returns>The current <see cref="IRowItem" /> instance.</returns>
    IRowItem HorizontalAlignment(HorizontalAlignment alignment);

    /// <summary>
    ///     Sets the vertical alignment for the row item.
    /// </summary>
    /// <param name="alignment">The desired vertical alignment.</param>
    /// <returns>The current <see cref="IRowItem" /> instance.</returns>
    IRowItem VerticalAlignment(VerticalAlignment alignment);

    internal List<string> LineItems { get; }

    internal int? MaxLength { get; }
    internal void UpdateLines(int charLength = 0);
    internal void AlignVertical(int minRows, int charLength);
}

internal sealed class RowItem : IRowItem
{
    private bool _textWrap;
    private HorizontalAlignment _horizontalAlignment = Sharprinter.HorizontalAlignment.Left;
    private VerticalAlignment _verticalAlignment = Sharprinter.VerticalAlignment.Top;

    private readonly string _data;

    public RowItem(string data, int maxLength)
    {
        _data = data.Replace("\n", " ").Replace("\r", " "); //sanitize
        MaxLength = maxLength;
    }

    public RowItem(string data)
    {
        _data = data.Replace("\n", " ").Replace("\r", " "); //sanitize
        MaxLength = null;
    }

    public int? MaxLength { get; }

    public IRowItem TextWrap()
    {
        _textWrap = true;
        return this;
    }

    public IRowItem HorizontalAlignment(HorizontalAlignment alignment)
    {
        _horizontalAlignment = alignment;
        return this;
    }

    public IRowItem VerticalAlignment(VerticalAlignment alignment)
    {
        _verticalAlignment = alignment;
        return this;
    }

    public List<string> LineItems { get; private set; } = [];

    /// <summary>
    ///     Updates the formatted lines for the item based on the specified total number of rows.
    ///     This method handles text wrapping, alignment, and padding to ensure the item fits properly
    ///     within the table structure.
    /// </summary>
    /// <param name="charLength">The required character length</param>
    public void UpdateLines(int charLength)
    {
        var appliedLength = MaxLength ?? charLength;

        if (string.IsNullOrEmpty(_data)) return;

        var formattedLines = new List<string>();
        if (_textWrap)
        {
            var lines = _data.SplitIntoLines(appliedLength);

            foreach (var line in lines)
            {
                var formated = FormatHorizontal(line, appliedLength);
                formattedLines.Add(formated);
            }
        }
        else
        {
            var line = _data.Length > appliedLength
                ? _data[..appliedLength]
                : FormatHorizontal(_data, appliedLength);

            formattedLines.Add(line);
        }

        LineItems = formattedLines;
    }

    private string FormatHorizontal(string line, int charLength)
    {
        var formatted = _horizontalAlignment switch
        {
            Sharprinter.HorizontalAlignment.Left => line.PadRight(charLength),
            Sharprinter.HorizontalAlignment.Center => line.PadLeft((charLength + line.Length) / 2).PadRight(charLength),
            Sharprinter.HorizontalAlignment.Right => line.PadLeft(charLength),
            _ => line.PadRight(charLength)
        };
        return formatted;
    }

    public void AlignVertical(int minRows, int charLength)
    {
        var items = LineItems;

        if (minRows <= items.Count) return;

        var excessRows = minRows - items.Count;

        if (_verticalAlignment == Sharprinter.VerticalAlignment.Top)
        {
            for (var i = 0; i < excessRows; i++) items.Add(new string(' ', charLength));
            return;
        }

        if (_verticalAlignment == Sharprinter.VerticalAlignment.Bottom)
        {
            for (var i = 0; i < excessRows; i++) items.Insert(0, new string(' ', charLength));
            return;
        }

        var topPadding = excessRows / 2;
        for (var i = 0; i < topPadding; i++)
        {
            items.Insert(0, new string(' ', charLength));
        }

        var bottomPadding = excessRows - topPadding;
        for (var i = 0; i < bottomPadding; i++)
        {
            items.Add(new string(' ', charLength));
        }
    }
}