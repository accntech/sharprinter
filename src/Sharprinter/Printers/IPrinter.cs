// ReSharper disable once CheckNamespace
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
    /// <param name="model">The printer model identifier. If empty or null, uses the default model.</param>
    void Initialize(string model = "");

    /// <summary>
    ///     Releases the printer resources and cleans up the printer handle.
    /// </summary>
    void Release();

    /// <summary>
    ///     Opens a communication port for the printer.
    /// </summary>
    /// <param name="port">The port identifier to open (e.g., "COM1", "USB001", "LPT1").</param>
    void OpenPort(string port);

    /// <summary>
    ///     Closes the currently open printer port.
    /// </summary>
    void ClosePort();

    /// <summary>
    ///     Finalizes the current print job before cutting, opening the cash drawer and releasing the resources.
    ///     This should be called after all print actions to complete the printing process.
    /// </summary>
    void FinalizePrint()
    {
    }

    /// <summary>
    ///     Cuts the paper using the specified cut mode.
    /// </summary>
    /// <param name="cutMode">The cut mode to use for paper cutting (typically 0 for full cut, 1 for partial cut).</param>
    void CutPaper(int cutMode);

    /// <summary>
    ///     Cuts the paper at a specified distance from the current position.
    /// </summary>
    /// <param name="distance">The distance in dots from the current position to cut the paper.</param>
    void CutPaperWithDistance(int distance);

    /// <summary>
    ///     Feeds the specified number of lines.
    ///     Note: This implementation uses PrintText with newline characters to ensure proper line spacing across different
    ///     printer models.
    /// </summary>
    /// <param name="lines">The number of lines to feed.</param>
    void FeedLine(int lines = 1);

    /// <summary>
    ///     Opens the cash drawer using the specified pin configuration.
    /// </summary>
    /// <param name="pinMode">The pin mode for the cash drawer (typically 0 for pin 2, 1 for pin 5).</param>
    /// <param name="onTime">The duration in milliseconds to keep the pin active.</param>
    /// <param name="ofTime">The duration in milliseconds to keep the pin inactive.</param>
    void OpenCashDrawer(int pinMode = 0, int onTime = 30, int ofTime = 255);

    /// <summary>
    ///     Prints text with line terminator, specified alignment and text size.
    ///     Note: The textWrap parameter is not used in the current implementation but is kept for interface compatibility.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="textWrap">Indicates whether text wrapping is enabled (not currently implemented).</param>
    /// <param name="alignment">The text alignment.</param>
    /// <param name="textSize">The text size multiplier.</param>
    void PrintText(string data, TextWrap textWrap = TextWrap.None,
        HorizontalAlignment alignment = HorizontalAlignment.Left, TextSize textSize = TextSize.Normal);

    /// <summary>
    ///     Prints a barcode with specified parameters.
    /// </summary>
    /// <param name="data">The barcode data to encode.</param>
    /// <param name="height">The barcode height in dots.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="alignment">The barcode alignment.</param>
    /// <param name="position">
    ///     The HRI (Human Readable Interpretation) position (0=Not printed, 1=Above, 2=Below, 3=Above and
    ///     Below).
    /// </param>
    void PrintBarCode(string data, int height, BarcodeWidth width, HorizontalAlignment alignment = HorizontalAlignment.Left, HRIPosition position = HRIPosition.None);

    /// <summary>
    ///     Prints an image from a file with specified scaling mode.
    ///     Note: The filename parameter is not used in the native implementation but is kept for interface compatibility.
    /// </summary>
    /// <param name="filePath">The path to the image file to print.</param>
    /// <param name="filename">Optional filename for the image (not used in native implementation).</param>
    /// <param name="scaleMode">
    ///     The scaling mode for the image (typically 0=Normal, 1=Double width, 2=Double height, 3=Double
    ///     width and height).
    /// </param>
    void PrintImage(string filePath, string filename, ScaleMode scaleMode);
}