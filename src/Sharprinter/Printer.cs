using System;
using System.Runtime.InteropServices;

namespace Sharprinter;

/// <summary>
///     Internal class providing low-level printer SDK function imports and wrapper methods.
/// </summary>
internal static class Printer
{
    /// <summary>
    ///     Initializes the printer with the specified model.
    /// </summary>
    /// <param name="model">The printer model identifier.</param>
    /// <returns>A pointer to the initialized printer instance.</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern IntPtr InitPrinter(string model);

    /// <summary>
    ///     Releases the printer resources and closes the connection.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern int ReleasePrinter(IntPtr intPtr);

    /// <summary>
    ///     Opens a communication port to the printer.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="port">The port configuration string (e.g., "COM1, 9600").</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern int OpenPort(IntPtr intPtr, string port);

    /// <summary>
    ///     Closes the communication port to the printer.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern int ClosePort(IntPtr intPtr);

    /// <summary>
    ///     Writes data to the printer.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="buffer">The data buffer to write.</param>
    /// <param name="size">The size of the data buffer.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern int WriteData(IntPtr intPtr, byte[] buffer, int size);

    /// <summary>
    ///     Reads data from the printer.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="buffer">The buffer to store the read data.</param>
    /// <param name="size">The size of the buffer.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern int ReadData(IntPtr intPtr, byte[] buffer, int size);

    /// <summary>
    ///     Initializes the printer for operation.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
    public static extern int PrinterInitialize(IntPtr intPtr);

    /// <summary>
    ///     Sets the line spacing for text printing.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="lineSpace">The line spacing value.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int SetTextLineSpace(IntPtr intPtr, int lineSpace);

    /// <summary>
    ///     Cancels print data in page mode.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int CancelPrintDataInPageMode(IntPtr intPtr);

    /// <summary>
    ///     Gets the current printer status.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="printerStatus">Reference to store the printer status.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int GetPrinterState(IntPtr intPtr, ref int printerStatus);

    /// <summary>
    ///     Sets the code page for character encoding.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="characterSet">The character set identifier.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int SetCodePage(IntPtr intPtr, int characterSet);

    /// <summary>
    ///     Sets the international character set.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="characterSet">The character set identifier.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int SetInternationalCharacter(IntPtr intPtr, int characterSet);

    /// <summary>
    ///     Cuts the paper using the specified cut mode.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="cutMode">The cut mode (0 for full cut, 1 for partial cut).</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int CutPaper(IntPtr intPtr, int cutMode);

    /// <summary>
    ///     Cuts the paper at the specified distance.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="distance">The distance before cutting.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int CutPaperWithDistance(IntPtr intPtr, int distance);

    /// <summary>
    ///     Feeds the paper by the specified number of lines.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="lines">The number of lines to feed.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int FeedLine(IntPtr intPtr, int lines);

    /// <summary>
    ///     Opens the cash drawer connected to the printer.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="pinMode">The pin mode for the cash drawer.</param>
    /// <param name="onTime">The on-time duration for the drawer signal.</param>
    /// <param name="ofTime">The off-time duration for the drawer signal.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int OpenCashDrawer(IntPtr intPtr, int pinMode, int onTime, int ofTime);

    /// <summary>
    ///     Prints text with the specified formatting options.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment (0=Left, 1=Center, 2=Right).</param>
    /// <param name="textSize">The text size multiplier.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int PrintText(IntPtr intPtr, string data, int alignment, int textSize);

    /// <summary>
    ///     Prints text with default formatting (simple version).
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="data">The text data to print.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int PrintTextS(IntPtr intPtr, string data);

    /// <summary>
    ///     Prints a barcode with the specified parameters.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="bcType">The barcode type identifier.</param>
    /// <param name="bcData">The barcode data to encode.</param>
    /// <param name="width">The width of the barcode.</param>
    /// <param name="height">The height of the barcode.</param>
    /// <param name="alignment">The barcode alignment (0=Left, 1=Center, 2=Right).</param>
    /// <param name="hriPosition">The HRI (Human Readable Interpretation) position.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int PrintBarCode(IntPtr intPtr, int bcType, string bcData, int width, int height,
        int alignment,
        int hriPosition);

    /// <summary>
    ///     Prints a symbol (QR code or other 2D symbols) with the specified parameters.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="type">The symbol type identifier.</param>
    /// <param name="data">The data to encode in the symbol.</param>
    /// <param name="errLevel">The error correction level.</param>
    /// <param name="width">The width of the symbol.</param>
    /// <param name="height">The height of the symbol.</param>
    /// <param name="alignment">The symbol alignment (0=Left, 1=Center, 2=Right).</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int PrintSymbol(IntPtr intPtr, int type, string data, int errLevel, int width, int height,
        int alignment);

    /// <summary>
    ///     Prints an image from the specified file path.
    /// </summary>
    /// <param name="intPtr">The printer instance pointer.</param>
    /// <param name="filePath">The file path of the image to print.</param>
    /// <param name="scaleMode">The scaling mode for the image.</param>
    /// <returns>An integer indicating the operation result (0 for success).</returns>
    [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int PrintImage(IntPtr intPtr, string filePath, int scaleMode);
}