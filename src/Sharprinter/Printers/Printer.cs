using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Provides a wrapper for the printer SDK functionality.
///     This class handles communication with thermal printers through the native SDK.
/// </summary>
public class Printer(int maxLineCharacter) : IPrinter
{
    private IntPtr _intPtr = IntPtr.Zero;

    /// <summary>
    ///     Initializes the printer with the specified model.
    /// </summary>
    /// <param name="model">Optional printer model identifier.</param>
    public void Initialize(string model = "")
    {
        _intPtr = Sdk.InitPrinter(model);
    }

    /// <summary>
    ///     Releases the printer resources.
    /// </summary>
    public void Release()
    {
        Sdk.ReleasePrinter(_intPtr);
    }

    /// <summary>
    ///     Opens the printer port with the specified port name.
    /// </summary>
    /// <param name="port">The port identifier to open.</param>
    public void OpenPort(string port)
    {
        Sdk.OpenPort(_intPtr, port);
    }

    /// <summary>
    ///     Closes the printer port.
    /// </summary>
    public void ClosePort()
    {
        Sdk.ClosePort(_intPtr);
    }

    /// <summary>
    ///     Cuts the paper using the specified cut mode.
    /// </summary>
    /// <param name="cutMode">The cut mode to use.</param>
    public void CutPaper(int cutMode)
    {
        Sdk.CutPaper(_intPtr, cutMode);
    }

    /// <summary>
    ///     Cuts the paper with a specified distance.
    /// </summary>
    /// <param name="distance">The distance for the paper cut operation.</param>
    public void CutPaperWithDistance(int distance)
    {
        Sdk.CutPaperWithDistance(_intPtr, distance);
    }

    /// <summary>
    ///     Feeds the specified number of lines.
    /// </summary>
    /// <param name="lines">The number of lines to feed.</param>
    public void FeedLine(int lines = 1)
    {
        // use PrintText to ensure proper line spacing with different printers
        Sdk.PrintText(_intPtr, new string('\n', lines), 0, 0);
    }

    /// <summary>
    ///     Opens the cash drawer with the specified parameters.
    /// </summary>
    /// <param name="pinMode">The pin mode for the cash drawer operation.</param>
    /// <param name="onTime">The on time duration for the cash drawer operation.</param>
    /// <param name="ofTime">The off time duration for the cash drawer operation.</param>
    public void OpenCashDrawer(int pinMode = 0, int onTime = 30, int ofTime = 255)
    {
        Sdk.OpenCashDrawer(_intPtr, pinMode, onTime, ofTime);
    }

    /// <summary>
    ///     Prints text with the specified alignment, text wrapping, and text size.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="textWrap">Indicates whether text wrapping is enabled.</param>
    /// <param name="alignment">The text alignment (e.g., left, center, right).</param>
    /// <param name="textSize">The text size to use for printing.</param>
    public void PrintText(string data, TextWrap textWrap, HorizontalAlignment alignment, TextSize textSize)
    {
        if (textWrap == TextWrap.None)
        {
            var trimmed = data.Length > maxLineCharacter
                ? data[..maxLineCharacter]
                : data;

            Sdk.PrintText(_intPtr, trimmed, 0, (int)textSize);
            return;
        }

        var lines = data.SplitIntoLines(maxLineCharacter);

        if (lines.Count == 1)
        {
            Sdk.PrintText(_intPtr, lines[0], 0, (int)textSize);
            return;
        }

        var counter = 0;
        foreach (var line in lines)
        {
            var formattedLine = alignment switch
            {
                HorizontalAlignment.Left => line, // Left alignment
                HorizontalAlignment.Center => line.PadLeft((maxLineCharacter + line.Length) / 2)
                    .PadRight(maxLineCharacter), // Center alignment
                HorizontalAlignment.Right => line.PadRight(maxLineCharacter), // Right alignment
                _ => line
            };

            Sdk.PrintText(_intPtr, counter < lines.Count - 1 ? $"{formattedLine}\n" : formattedLine, 0, (int)textSize); // Last line without newline
            counter++;
        }
    }

    /// <summary>
    ///     Prints a line of text with the specified alignment, text wrapping, and text size, followed by a newline.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="textWrap">Indicates whether text wrapping is enabled.</param>
    /// <param name="alignment">The text alignment (e.g., left, center, right).</param>
    /// <param name="textSize">The text size to use for printing.</param>
    public void PrintTextLine(string data, TextWrap textWrap, HorizontalAlignment alignment, TextSize textSize)
    {
        if (textWrap == TextWrap.None)
        {
            var trimmed = data.Length > maxLineCharacter
                ? data[..maxLineCharacter]
                : data;

            var line = alignment switch
            {
                HorizontalAlignment.Left => trimmed, // Left alignment
                HorizontalAlignment.Center => trimmed.PadLeft((maxLineCharacter + trimmed.Length) / 2).PadRight(maxLineCharacter), // Center alignment
                HorizontalAlignment.Right => trimmed.PadRight(maxLineCharacter), // Right alignment
                _ => trimmed
            };
            Sdk.PrintText(_intPtr, $"{line}\n", 0, (int)textSize);
            return;
        }

        var lines = data.SplitIntoLines(maxLineCharacter);

        foreach (var line in lines)
        {
            var formattedLine = alignment switch
            {
                HorizontalAlignment.Left => line, // Left alignment
                HorizontalAlignment.Center => line.PadLeft((maxLineCharacter + line.Length) / 2).PadRight(maxLineCharacter), // Center alignment
                HorizontalAlignment.Right => line.PadRight(maxLineCharacter), // Right alignment
                _ => line
            };

            Sdk.PrintText(_intPtr, $"{formattedLine}\n", 0, (int)textSize);
        }
    }

    /// <summary>
    ///     Prints a barcode with the specified parameters.
    /// </summary>
    /// <param name="data">The barcode data.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="height">The barcode height.</param>
    /// <param name="alignment">The barcode alignment.</param>
    /// <param name="position">The barcode position.</param>
    public void PrintBarCode(string data, int height, BarcodeWidth width = BarcodeWidth.Large, HorizontalAlignment alignment = HorizontalAlignment.Left, HRIPosition position = HRIPosition.None)
    {
        Sdk.PrintBarCode(_intPtr, 73, data, (int)width, height, (int)alignment, (int)position);
    }

    /// <summary>
    ///     Prints an image with the specified file path, filename, and scale mode.
    /// </summary>
    /// <param name="filePath">The file path of the image.</param>
    /// <param name="filename">The filename to display (not used in native implementation).</param>
    /// <param name="scaleMode">The scale mode for the image.</param>
    public void PrintImage(string filePath, string filename, ScaleMode scaleMode)
    {
        Sdk.PrintImage(_intPtr, filePath, (int)scaleMode);
    }

    private static class Sdk
    {
        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern IntPtr InitPrinter(string model);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int ReleasePrinter(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OpenPort(IntPtr intPtr, string port);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int ClosePort(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int WriteData(IntPtr intPtr, byte[] buffer, int size);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int ReadData(IntPtr intPtr, byte[] buffer, int size);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int PrinterInitialize(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int SetTextLineSpace(IntPtr intPtr, int lineSpace);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int CancelPrintDataInPageMode(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int GetPrinterState(IntPtr intPtr, ref int printerStatus);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int SetCodePage(IntPtr intPtr, int characterSet);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int SetInternationalCharacter(IntPtr intPtr, int characterSet);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int CutPaper(IntPtr intPtr, int cutMode);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int CutPaperWithDistance(IntPtr intPtr, int distance);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FeedLine(IntPtr intPtr, int lines);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int OpenCashDrawer(IntPtr intPtr, int pinMode, int onTime, int ofTime);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintText(IntPtr intPtr, string data, int alignment, int textSize);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintTextS(IntPtr intPtr, string data);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintBarCode(IntPtr intPtr, int bcType, string bcData, int width, int height,
            int alignment,
            int hriPosition);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintSymbol(IntPtr intPtr, int type, string data, int errLevel, int width, int height,
            int alignment);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintImage(IntPtr intPtr, string filePath, int scaleMode);
    }
}