using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Internal implementation of the <see cref="ITable" /> interface for creating formatted tables.
/// </summary>
internal sealed class Table(PrinterContext context, int maxCharCount) : ITable
{
    private readonly List<Action> _printActions = [];

    public ITable AddLine(LineItem[] lines)
    {
        _printActions.Add(() =>
        {
            var maxRows = lines.Max(x => x.Lines.Count);
            foreach (var line in lines) line.UpdateLines(maxRows);

            var index = 0;
            while (index < maxRows)
            {
                var groupedLines = lines.Select(x => x.Lines.ElementAtOrDefault(index) ?? string.Empty).ToArray();
                var lineString = string.Join("", groupedLines);
                context.Printer.PrintTextLine(lineString);
                index++;
            }
        });

        return this;
    }

    public ITable AddLine(string label, bool textWrap = true, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left)
    {
        AddLine([new LineItem(label, maxCharCount, horizontalAlignment, VerticalAlignment.Top, textWrap)]);
        return this;
    }

    public ITable AddLine(string label, string value, int minValueLength = 15)
    {
        var valueLength = Math.Max(minValueLength, value.Length) + 1;
        var labelLength = maxCharCount - valueLength; // -1 for space
        AddLine([
            new LineItem(label, labelLength),
            new LineItem($"{value}", valueLength, HorizontalAlignment.Right)
        ]);

        return this;
    }

    public ITable AddEmptyLine(int rows = 1)
    {
        _printActions.Add(() => context.Printer.FeedLine(rows));
        return this;
    }

    public ITable AddRowSeparator()
    {
        _printActions.Add(() =>
            context.Printer.PrintTextLine(new string(Border.HorizontalLine, maxCharCount)));
        return this;
    }

    public PrinterContext Create()
    {
        context.AddTableActions(_printActions);
        return context;
    }
}