using System;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Defines the contract for creating and formatting tables for printing.
/// </summary>
public interface ITable
{
    /// <summary>
    ///     Adds a new row to the table using the specified configuration action.
    /// </summary>
    /// <param name="expression">An action that configures the row by adding cells and formatting.</param>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddRow(Action<IRow> expression);

    /// <summary>
    ///     Adds an empty row to the table.
    /// </summary>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable FeedLine(int rows = 1);

    /// <summary>
    ///     Adds a horizontal separator row to the table.
    /// </summary>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddSeparator();
}

/// <summary>
///     Internal implementation of the <see cref="ITable" /> interface for creating formatted tables.
/// </summary>
internal sealed class Table(PrinterContext context, int maxCharCount) : ITable
{
    public ITable AddRow(Action<IRow> expression)
    {
        var row = new Row(maxCharCount);
        expression.Invoke(row);

        var items = row.GetLineItems();
        foreach (var item in items) context.Printer.PrintText(item);

        return this;
    }

    public ITable FeedLine(int rows = 1)
    {
        context.Printer.FeedLine(rows);
        return this;
    }

    public ITable AddSeparator()
    {
        context.Printer.PrintText(new string(Border.HorizontalLine, maxCharCount));
        return this;
    }
}