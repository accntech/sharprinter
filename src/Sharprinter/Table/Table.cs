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

    public ITable AddHeader(HeaderItem[] headers)
    {
        //print top border
        _printActions.Add(() => context.Printer.PrintText(string.Empty.PadLeft(maxCharCount, '-') + "\n\r", 0, 0));

        //print header
        _printActions.Add(() =>
        {
            var header = headers.Aggregate("", (current, header) =>
            {
                var label = header.Label;
                var maxPaddingLength = header.MaxLength - label.Length < 0 ? 0 : header.MaxLength - label.Length;
                var padding = string.Empty.PadLeft(maxPaddingLength, ' ');
                var centerPadding = string.Empty.PadLeft(maxPaddingLength / 2, ' ');
                return header.Alignment switch
                {
                    Alignment.Left => $"{current}{label}{padding}",
                    Alignment.Center => $"{current}{centerPadding}{label}{centerPadding}",
                    Alignment.Right => $"{current}{padding}{label}",
                    _ => $"{current}{label}{padding}"
                };
            });

            context.Printer.PrintText(header + "\n\r", 0, 0);
        });

        //print bottom border
        _printActions.Add(() => context.Printer.PrintText(string.Empty.PadLeft(maxCharCount, '-') + "\n\r", 0, 0));

        return this;
    }

    public ITable AddLine(LineItem[] lines)
    {
        //print line
        _printActions.Add(() =>
        {
            var line = lines.Aggregate("", (current, line) =>
            {
                var label = line.Label;
                var maxPaddingLength = line.MaxLength - label.Length < 0 ? 0 : line.MaxLength - label.Length;
                var padding = string.Empty.PadLeft(maxPaddingLength, ' ');
                var centerPadding = string.Empty.PadLeft(maxPaddingLength / 2, ' ');

                return line.Alignment switch
                {
                    Alignment.Left => $"{current}{label[..Math.Min(label.Length, line.MaxLength)]}{padding}",
                    Alignment.Center =>
                        $"{current}{centerPadding}{label[..Math.Min(label.Length, line.MaxLength)]}{centerPadding}",
                    Alignment.Right => $"{current}{padding}{label[..Math.Min(label.Length, line.MaxLength)]}",
                    _ => $"{current}{label[..Math.Min(label.Length, line.MaxLength)]}{padding}"
                };
            }); //TODO: Add text wrap

            context.Printer.PrintText(line, 0, 0);
        });

        return this;
    }

    public ITable AddLine(string label)
    {
        AddLine([new LineItem(label, maxCharCount)]);
        return this;
    }

    public ITable AddLine(string label, string value, int minValueLength = 15)
    {
        var valueLength = Math.Max(minValueLength, value.Length) + 1;
        var labelLength = maxCharCount - valueLength; // -1 for space
        AddLine([
            new LineItem(label, labelLength),
            new LineItem($"{value}", valueLength, Alignment.Right)
        ]);

        return this;
    }

    public ITable AddEmptyLine()
    {
        _printActions.Add(() => context.Printer.PrintText(" \n\r", 0, 0));
        return this;
    }

    public ITable AddRowSeparator()
    {
        _printActions.Add(() => context.Printer.PrintText(string.Empty.PadLeft(maxCharCount, '-'), 0, 0));
        return this;
    }

    public PrinterContext Create()
    {
        _printActions.Add(() => context.Printer.PrintText(string.Empty.PadLeft(maxCharCount, '-'), 0, 0));
        context.AddTableActions(_printActions);

        return context;
    }
}