namespace Sharprinter;

/// <summary>
///     Configuration options for printer behavior and connection settings.
/// </summary>
public sealed class PrinterOptions
{
    /// <summary>
    ///     Gets or sets the maximum number of characters per line for text wrapping and formatting.
    /// </summary>
    public int MaxLineCharacter { get; init; }

    /// <summary>
    ///     Gets or sets the name of the communication port to connect to the printer.
    /// </summary>
    public string PortName { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the baud rate for serial communication with the printer.
    ///     Default value is 9600.
    /// </summary>
    public int BaudRate { get; init; } = 9600;

    /// <summary>
    ///     Gets or sets a value indicating whether the cash drawer should be opened after printing.
    /// </summary>
    public bool OpenDrawer { get; init; }

    /// <summary>
    ///     Gets or sets a value indicating whether the paper should be cut after printing.
    /// </summary>
    public bool CutPaper { get; init; }
}