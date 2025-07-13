using System;
using System.Text;

namespace Sharprinter;

/// <summary>
///     A console-based implementation of the IPrinter interface that simulates printer operations
///     by outputting formatted text to the console with visual borders and formatting.
/// </summary>
/// <param name="maxLineCharacter">The maximum number of characters per line for the receipt output.</param>
public class ConsolePrinter(int maxLineCharacter) : IPrinter
{
    private const int Padding = 8; // Padding for console output

    private const char TopLeft = '╭';
    private const char TopRight = '╮';
    private const char BottomLeft = '╰';
    private const char BottomRight = '╯';
    private const char HorizontalLine = '─';
    private const char VerticalLine = '│';

    /// <summary>
    ///     Initializes the console printer and sets up UTF-8 encoding for proper character display.
    /// </summary>
    /// <param name="model">Optional printer model identifier (not used in console implementation).</param>
    public void Initialize(string model = "")
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Printer initialized.");
        Console.WriteLine();
    }

    /// <summary>
    ///     Releases the console printer resources and outputs a confirmation message.
    /// </summary>
    public void Release()
    {
        Console.WriteLine("Printer released successfully.");
        Console.WriteLine();
    }

    /// <summary>
    ///     Opens a console port and displays a top border for the receipt output.
    /// </summary>
    /// <param name="port">The port identifier (not used in console implementation).</param>
    public void OpenPort(string port)
    {
        Console.WriteLine("Console port opened.");
        Console.WriteLine();

        var line = new string(HorizontalLine, maxLineCharacter + Padding - 2);
        Console.WriteLine($"{TopLeft}{line}{TopRight}");
    }

    /// <summary>
    ///     Closes the console port and displays a bottom border for the receipt output.
    /// </summary>
    public void ClosePort()
    {
        var line = new string(HorizontalLine, maxLineCharacter + Padding - 2);
        Console.WriteLine($"{BottomLeft}{line}{BottomRight}");
        Console.WriteLine();
        Console.WriteLine("Console port closed.");
    }

    /// <summary>
    ///     Simulates a paper cut operation by outputting a confirmation message.
    /// </summary>
    /// <param name="cutMode">The cut mode to use (not used in console implementation).</param>
    public void CutPaper(int cutMode)
    {
        Console.WriteLine("Paper cut requested.");
        Console.WriteLine();
    }

    /// <summary>
    ///     Simulates a paper cut operation with a specified distance by outputting a confirmation message.
    /// </summary>
    /// <param name="distance">The distance for the paper cut operation.</param>
    public void CutPaperWithDistance(int distance)
    {
        Console.WriteLine($"Paper cut with distance {distance} requested.");
        Console.WriteLine();
    }

    /// <summary>
    ///     Feeds the specified number of empty lines to simulate paper feed.
    /// </summary>
    /// <param name="lines">The number of lines to feed.</param>
    public void FeedLine(int lines)
    {
        var line = 0;
        while (line < lines)
        {
            Console.WriteLine(ReceiptText(string.Empty, maxLineCharacter));
            line++;
        }
    }

    /// <summary>
    ///     Simulates opening a cash drawer by outputting a confirmation message.
    /// </summary>
    /// <param name="pinMode">The pin mode for the cash drawer operation.</param>
    /// <param name="onTime">The on time duration for the cash drawer operation.</param>
    /// <param name="ofTime">The off time duration for the cash drawer operation.</param>
    public void OpenCashDrawer(int pinMode, int onTime, int ofTime)
    {
        Console.WriteLine("Open cash drawer requested");
        Console.WriteLine();
    }

    /// <summary>
    ///     Prints text with specified alignment and text size to the console with visual borders.
    ///     The text is automatically wrapped to fit within the maximum line character limit.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment: 0 for left, 1 for center, 2 for right.</param>
    /// <param name="textSize">The text size (not used in console implementation).</param>
    public void PrintText(string data, int alignment, int textSize)
    {
        var lines = data.SplitIntoLines(maxLineCharacter);

        if (lines.Count == 1)
        {
            Console.Write(ReceiptText(lines[0], maxLineCharacter));
            return;
        }

        var counter = 0;
        foreach (var line in lines)
        {
            var formattedLine = alignment switch
            {
                0 => line, // Left alignment
                1 => line.PadLeft((maxLineCharacter + line.Length) / 2).PadRight(maxLineCharacter), // Center alignment
                2 => line.PadRight(maxLineCharacter), // Right alignment
                _ => line
            };

            if (counter < lines.Count - 1)
            {
                Console.WriteLine(ReceiptText(formattedLine, maxLineCharacter));
            }
            else
            {
                Console.Write(ReceiptText(formattedLine, maxLineCharacter)); // Last line without newline
            }

            counter++;
        }
    }

    /// <summary>
    ///     Formats text with vertical borders and padding for receipt-style output.
    /// </summary>
    /// <param name="text">The text to format.</param>
    /// <param name="maxChar">The maximum number of characters per line.</param>
    /// <returns>A formatted string with vertical borders and padding.</returns>
    private string ReceiptText(string text, int maxChar)
    {
        var paddedLeft = $"   {text}";
        var paddedRight = paddedLeft.PadRight(maxChar + Padding - 2);

        return $"{VerticalLine}{paddedRight}{VerticalLine}";
    }

    /// <summary>
    ///     Prints text with specified alignment and text size to the console with visual borders.
    ///     Each line of text is printed on a separate line with a newline character.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment: 0 for left, 1 for center, 2 for right.</param>
    /// <param name="textSize">The text size (not used in console implementation).</param>
    public void PrintTextLine(string data, int alignment, int textSize)
    {
        var lines = data.SplitIntoLines(maxLineCharacter);

        foreach (var line in lines)
        {
            var formattedLine = alignment switch
            {
                0 => line, // Left alignment
                1 => line.PadLeft((maxLineCharacter + line.Length) / 2).PadRight(maxLineCharacter), // Center alignment
                2 => line.PadRight(maxLineCharacter), // Right alignment
                _ => line
            };

            Console.WriteLine(ReceiptText(formattedLine, maxLineCharacter));
        }
    }

    /// <summary>
    ///     Prints a barcode with specified parameters. This method is not implemented in the console version.
    /// </summary>
    /// <param name="type">The barcode type.</param>
    /// <param name="data">The barcode data.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="height">The barcode height.</param>
    /// <param name="alignment">The barcode alignment.</param>
    /// <param name="position">The barcode position.</param>
    /// <exception cref="NotImplementedException">Thrown because barcode printing is not implemented in the console version.</exception>
    public void PrintBarCode(int type, string data, int width, int height, int alignment, int position)
    {
        var lineChar = new string(HorizontalLine, maxLineCharacter - 2);
        Console.WriteLine(ReceiptText($"{TopLeft}{lineChar}{TopRight}", maxLineCharacter));

        var maxChar = maxLineCharacter - 8;
        var remainingData = data.Length >= maxChar ? data[..maxChar] : data;
        var barcode = GenerateDummyBarcode(remainingData);

        var formatted = alignment switch
        {
            0 => barcode, // Left alignment
            1 => barcode.PadLeft((maxChar + barcode.Length) / 2).PadRight(maxChar), // Center alignment
            2 => barcode.PadRight(maxChar), // Right alignment
            _ => barcode
        };

        var croppedBarcode = formatted.Length > maxLineCharacter
            ? formatted[..maxLineCharacter]
            : formatted;
        Console.WriteLine(ReceiptText(croppedBarcode, maxLineCharacter));

        var label = data.Length >= maxLineCharacter ? data[..maxLineCharacter] : data;
        var centeredLabel = label.PadLeft((maxChar + label.Length) / 2).PadRight(maxChar);
        Console.WriteLine(ReceiptText(ReceiptText(centeredLabel, maxChar), maxLineCharacter));

        Console.WriteLine(ReceiptText($"{BottomLeft}{lineChar}{BottomRight}", maxLineCharacter));
    }

    private static string GenerateDummyBarcode(string input)
    {
        var bar = "";

        foreach (var c in input)
        {
            var num = c - '0';

            // Use different block patterns based on digit
            var barBlock = num % 2 == 0 ? "│" : "││";
            bar += new string(barBlock[0], num % 4 + 1); // repeat block
            bar += " ";
        }

        return bar.TrimEnd();
    }

    /// <summary>
    ///     Prints an image representation to the console with a bordered frame and filename display.
    /// </summary>
    /// <param name="filePath">The file path of the image (not used in console implementation).</param>
    /// <param name="filename">The filename to display in the image frame.</param>
    /// <param name="scaleMode">The scale mode for the image (not used in console implementation).</param>
    public void PrintImage(string filePath, string filename, int scaleMode)
    {
        var lineChar = new string(HorizontalLine, maxLineCharacter - 2);
        Console.WriteLine(ReceiptText($"{TopLeft}{lineChar}{TopRight}", maxLineCharacter));

        var width = maxLineCharacter - 8;
        var lines = filename.SplitIntoLines(width);
        foreach (var line in lines)
        {
            var formatted = line.PadLeft((width + line.Length) / 2).PadRight(width);
            Console.WriteLine(ReceiptText(ReceiptText(formatted, width), maxLineCharacter));
        }

        Console.WriteLine(ReceiptText($"{BottomLeft}{lineChar}{BottomRight}", maxLineCharacter));
    }
}