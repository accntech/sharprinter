// ReSharper disable once CheckNamespace

namespace Sharprinter;

/// <summary>
///     Defines the contract for creating and formatting tables for printing.
/// </summary>
public interface ITable
{
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
    /// <param name="textWrap">Indicates whether the text should wrap within the row.</param>
   /// <param name="horizontalAlignment">Specifies the horizontal alignment of the text within the row.</param>
    /// <returns>The current <see cref="ITable" /> instance for method chaining.</returns>
    ITable AddLine(string label, bool textWrap = true, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left);

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
    ITable AddEmptyLine(int rows = 1);

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