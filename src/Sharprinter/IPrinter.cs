namespace Sharprinter;

/// <summary>
///     Defines the interface for thermal printer operations.
///     This interface provides a contract for printer initialization, communication, and printing operations.
/// </summary>
public interface IPrinter
{
    /// <summary>
    ///     Initializes the printer with the specified model.
    /// </summary>
    /// <param name="model">The printer model identifier. If empty, uses the default model.</param>
    void Initialize(string model = "");

    /// <summary>
    ///     Releases the printer resources and cleans up the printer handle.
    /// </summary>
    void Release();

    /// <summary>
    ///     Opens a communication port for the printer.
    /// </summary>
    /// <param name="port">The port identifier to open (e.g., "COM1", "USB001").</param>
    void OpenPort(string port);

    /// <summary>
    ///     Closes the currently open printer port.
    /// </summary>
    void ClosePort();

    /// <summary>
    ///     Cuts the paper using the specified cut mode.
    /// </summary>
    /// <param name="cutMode">The cut mode to use for paper cutting.</param>
    void CutPaper(int cutMode);

    /// <summary>
    ///     Cuts the paper at a specified distance from the current position.
    /// </summary>
    /// <param name="distance">The distance in dots from the current position to cut the paper.</param>
    void CutPaperWithDistance(int distance);

    /// <summary>
    ///     Feeds the specified number of lines.
    /// </summary>
    /// <param name="lines">The number of lines to feed.</param>
    void FeedLine(int lines);

    /// <summary>
    ///     Opens the cash drawer using the specified pin configuration.
    /// </summary>
    /// <param name="pinMode">The pin mode for the cash drawer.</param>
    /// <param name="onTime">The duration in milliseconds to keep the pin active.</param>
    /// <param name="ofTime">The duration in milliseconds to keep the pin inactive.</param>
    void OpenCashDrawer(int pinMode, int onTime, int ofTime);

    /// <summary>
    ///     Prints text with specified alignment and text size.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment (0=Left, 1=Center, 2=Right).</param>
    /// <param name="textSize">The text size multiplier.</param>
    void PrintText(string data, int alignment, int textSize);
    
    /// <summary>
    ///     Prints text with line terminator, specified alignment and text size .
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment (0=Left, 1=Center, 2=Right).</param>
    /// <param name="textSize">The text size multiplier.</param>
    void PrintTextLine(string data, int alignment, int textSize);

    /// <summary>
    ///     Prints a barcode with specified parameters.
    /// </summary>
    /// <param name="type">The barcode type.</param>
    /// <param name="data">The barcode data to encode.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="height">The barcode height.</param>
    /// <param name="alignment">The barcode alignment (0=Left, 1=Center, 2=Right).</param>
    /// <param name="position">The HRI (Human Readable Interpretation) position.</param>
    void PrintBarCode(int type, string data, int width, int height, int alignment, int position);

    /// <summary>
    ///     Prints an image from a file with specified scaling mode.
    /// </summary>
    /// <param name="filePath">The path to the image file to print.</param>
    /// <param name="scaleMode">The scaling mode for the image.</param>
    void PrintImage(string filePath, int scaleMode);
}