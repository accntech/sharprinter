// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Defines the contract for creating and formatting tables for printing.
/// </summary>
public interface ITable
{
    /// <summary>
    ///     Adds a header row to the table with the specified header items.
    /// </summary>
    /// <param name="headers">Array of header items defining the column headers.</param>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddHeader(HeaderItem[] headers);

    /// <summary>
    ///     Adds a data row to the table with the specified line items.
    /// </summary>
    /// <param name="lines">Array of line items defining the row data.</param>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddLine(LineItem[] lines);

    /// <summary>
    ///     Adds a single-column data row to the table with the specified label.
    /// </summary>
    /// <param name="label">The text content for the row.</param>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddLine(string label);

    /// <summary>
    ///     Adds a two-column data row to the table with a label and value.
    /// </summary>
    /// <param name="label">The label text for the left column.</param>
    /// <param name="value">The value text for the right column.</param>
    /// <param name="minValueLength">The minimum length for the value column (default is 15).</param>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddLine(string label, string value, int minValueLength = 15);

    /// <summary>
    ///     Adds an empty row to the table.
    /// </summary>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddEmptyLine();

    /// <summary>
    ///     Adds a horizontal separator row to the table.
    /// </summary>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddRowSeparator();

    /// <summary>
    ///     Finalizes the table construction and returns the printer context for further operations.
    /// </summary>
    /// <returns>The <see cref="PrinterContext" /> instance containing all table print actions.</returns>
    PrinterContext Create();
}