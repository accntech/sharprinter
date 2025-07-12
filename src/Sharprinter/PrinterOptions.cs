namespace Sharprinter;

/// <summary>
///     Configuration options for printer behavior and connection settings.
/// </summary>
public sealed class PrinterOptions
{
    /// <summary>
    ///     Gets or sets the maximum number of characters per line for text wrapping and formatting.
    /// </summary>
    public int MaxLineCharacter { get; set; }

    /// <summary>
    ///     Gets or sets the name of the communication port to connect to the printer.
    /// </summary>
    public string PortName { get; set; }

    /// <summary>
    ///     Gets or sets the baud rate for serial communication with the printer.
    ///     Default value is 9600.
    /// </summary>
    public int BaudRate { get; set; } = 9600;

    /// <summary>
    ///     Gets or sets a value indicating whether the cash drawer should be opened after printing.
    ///     Default value is false.
    /// </summary>
    public bool OpenDrawer { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the paper should be cut after printing.
    ///     Default value is false.
    /// </summary>
    public bool CutPaper { get; set; } = false;
}