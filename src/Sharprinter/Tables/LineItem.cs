using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Represents a data item for table rows with label, maximum length, alignment, and text wrap properties.
///     This class handles the formatting and display of individual cells within a table row.
/// </summary>
public sealed class LineItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LineItem" /> class.
    /// </summary>
    /// <param name="label">The label or content of the row item.</param>
    /// <param name="maxLength">The maximum character length for the item column.</param>
    /// <param name="horizontalAlignment">
    ///     The horizontal alignment of the text within the column. Default is
    ///     <see cref="HorizontalAlignment.Left" />.
    /// </param>
    /// <param name="verticalAlignment">
    ///     The vertical alignment of the text within the row. Default is
    ///     <see cref="VerticalAlignment.Top" />.
    /// </param>
    /// <param name="textWrap">Indicates whether text wrapping is enabled for the item. Default is <c>false</c>.</param>
    public LineItem(
        string label,
        int maxLength,
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment verticalAlignment = VerticalAlignment.Top,
        bool textWrap = false)
    {
        Label = label;
        MaxLength = maxLength;
        HorizontalAlignment = horizontalAlignment;
        VerticalAlignment = verticalAlignment;
        TextWrap = textWrap;

        UpdateLines(1); // Initialize lines with a single row
    }

    /// <summary>
    ///     Gets the list of formatted lines for the item.
    ///     Each line is formatted according to the horizontal alignment and maximum length constraints.
    /// </summary>
    public List<string> Lines { get; private set; } = [];

    /// <summary>
    ///     Gets the label or content text of the row item.
    /// </summary>
    public string Label { get; }

    /// <summary>
    ///     Gets the maximum character length for the item column.
    ///     Text exceeding this length will be truncated or wrapped depending on the <see cref="TextWrap" /> setting.
    /// </summary>
    public int MaxLength { get; }

    /// <summary>
    ///     Gets a value indicating whether text wrapping is enabled for the item.
    ///     When <c>true</c>, text longer than <see cref="MaxLength" /> will be split into multiple lines.
    ///     When <c>false</c>, text will be truncated to fit within <see cref="MaxLength" />.
    /// </summary>
    public bool TextWrap { get; }

    /// <summary>
    ///     Gets the horizontal text alignment for the item within its column.
    ///     Determines how text is positioned horizontally when it's shorter than <see cref="MaxLength" />.
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; }

    /// <summary>
    ///     Gets the vertical alignment for the item within its row.
    ///     Determines how the item is positioned vertically when the row height exceeds the content height.
    /// </summary>
    public VerticalAlignment VerticalAlignment { get; }

    /// <summary>
    ///     Updates the formatted lines for the item based on the specified total number of rows.
    ///     This method handles text wrapping, alignment, and padding to ensure the item fits properly
    ///     within the table structure.
    /// </summary>
    /// <param name="totalRows">The total number of rows available for this item.</param>
    public void UpdateLines(int totalRows)
    {
        if (string.IsNullOrEmpty(Label)) return;

        var formattedLines = new List<string>();
        if (TextWrap)
        {
            var lines = Label.SplitIntoLines(MaxLength);

            foreach (var line in lines)
            {
                var formated = FormatHorizontal(line);
                formattedLines.Add(formated);
            }
        }
        else
        {
            var line = Label.Length > MaxLength
                ? Label[..MaxLength]
                : FormatHorizontal(Label);

            formattedLines.Add(line);
        }

        Lines = AlignVertical(totalRows, formattedLines);
    }

    private string FormatHorizontal(string line)
    {
        var formatted = HorizontalAlignment switch
        {
            HorizontalAlignment.Left => line.PadRight(MaxLength),
            HorizontalAlignment.Center => line.PadLeft((MaxLength + line.Length) / 2).PadRight(MaxLength),
            HorizontalAlignment.Right => line.PadLeft(MaxLength),
            _ => line.PadRight(MaxLength)
        };
        return formatted;
    }

    private List<string> AlignVertical(int requiredRows, List<string> items)
    {
        if (requiredRows <= items.Count)
        {
            return items;
        }

        var excessRows = requiredRows - items.Count;

        if (VerticalAlignment == VerticalAlignment.Top)
        {
            for (var i = 0; i < excessRows; i++) items.Add(new string(' ', MaxLength));
            return items;
        }

        if (VerticalAlignment == VerticalAlignment.Bottom)
        {
            for (var i = 0; i < excessRows; i++) items.Insert(0, new string(' ', MaxLength));
            return items;
        }

        var topPadding = excessRows / 2;
        for (var i = 0; i < topPadding; i++)
        {
            items.Insert(0, new string(' ', MaxLength));
        }

        var bottomPadding = excessRows - topPadding;
        for (var i = 0; i < bottomPadding; i++)
        {
            items.Add(new string(' ', MaxLength));
        }

        return items;
    }
}