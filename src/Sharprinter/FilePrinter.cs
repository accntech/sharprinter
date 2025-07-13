using System;
using System.IO;

namespace Sharprinter;

/// <summary>
///     A file-based implementation of the <see cref="IPrinter" /> interface that writes formatted receipt output to a
///     file.
/// </summary>
/// <param name="writer">The <see cref="StreamWriter" /> used to write output to the file.</param>
/// <param name="maxLineCharacter">The maximum number of characters per line for the receipt output.</param>
public class FilePrinter(StreamWriter writer, int maxLineCharacter) : IPrinter
{
    /// <summary>
    ///     Initializes the file printer and writes a header message to the file.
    /// </summary>
    /// <param name="model">Optional printer model identifier (not used in file implementation).</param>
    public void Initialize(string model = "")
    {
        writer.WriteLine("Printer initialized.");
        writer.WriteLine();
    }

    /// <summary>
    ///     Releases the file printer resources, closes the file, and opens it in the default text editor.
    /// </summary>
    public void Release()
    {
        writer.WriteLine("Printer released successfully.");
        writer.WriteLine();
        writer.Close();
        writer.Dispose();
    }

    /// <summary>
    ///     Opens a file port and displays a top border for the receipt output.
    /// </summary>
    /// <param name="port">The port identifier (not used in file implementation).</param>
    public void OpenPort(string port)
    {
        writer.WriteLine("File port opened.");
        writer.WriteLine();

        var line = new string(Border.HorizontalLine, maxLineCharacter + Border.Padding - 2);
        writer.WriteLine($"{Border.TopLeft}{line}{Border.TopRight}");
    }

    /// <summary>
    ///     Closes the file port and displays a bottom border for the receipt output.
    /// </summary>
    public void ClosePort()
    {
        var line = new string(Border.HorizontalLine, maxLineCharacter + Border.Padding - 2);
        writer.WriteLine($"{Border.BottomLeft}{line}{Border.BottomRight}");
        writer.WriteLine();
        writer.WriteLine("File port closed.");
        writer.Close();
        writer.Dispose();
    }

    /// <summary>
    ///     Simulates a paper cut operation by writing a confirmation message to the file.
    /// </summary>
    /// <param name="cutMode">The cut mode to use (not used in file implementation).</param>
    public void CutPaper(int cutMode)
    {
        writer.WriteLine("Paper cut requested.");
        writer.WriteLine();
    }

    /// <summary>
    ///     Simulates a paper cut operation with a specified distance by writing a confirmation message to the file.
    /// </summary>
    /// <param name="distance">The distance for the paper cut operation.</param>
    public void CutPaperWithDistance(int distance)
    {
        writer.WriteLine($"Paper cut with distance {distance} requested.");
        writer.WriteLine();
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
            writer.WriteLine(ReceiptText(string.Empty, maxLineCharacter));
            line++;
        }
    }

    /// <summary>
    ///     Simulates opening a cash drawer by writing a confirmation message to the file.
    /// </summary>
    /// <param name="pinMode">The pin mode for the cash drawer operation.</param>
    /// <param name="onTime">The on time duration for the cash drawer operation.</param>
    /// <param name="ofTime">The off time duration for the cash drawer operation.</param>
    public void OpenCashDrawer(int pinMode, int onTime, int ofTime)
    {
        writer.WriteLine("Open cash drawer requested");
        writer.WriteLine();
    }

    /// <summary>
    ///     Prints text with specified alignment and text size to the file with visual borders.
    ///     The text is automatically wrapped to fit within the maximum line character limit.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment: 0 for left, 1 for center, 2 for right.</param>
    /// <param name="textSize">The text size (not used in file implementation).</param>
    public void PrintText(string data, int alignment, int textSize)
    {
        var lines = data.SplitIntoLines(maxLineCharacter);

        if (lines.Count == 1)
        {
            writer.Write(ReceiptText(lines[0], maxLineCharacter));
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
                writer.WriteLine(ReceiptText(formattedLine, maxLineCharacter));
            }
            else
            {
                writer.Write(ReceiptText(formattedLine, maxLineCharacter)); // Last line without newline
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
        var paddedRight = paddedLeft.PadRight(maxChar + Border.Padding - 2);

        return $"{Border.VerticalLine}{paddedRight}{Border.VerticalLine}";
    }

    /// <summary>
    ///     Prints text with specified alignment and text size to the file with visual borders.
    ///     Each line of text is printed on a separate line with a newline character.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="alignment">The text alignment: 0 for left, 1 for center, 2 for right.</param>
    /// <param name="textSize">The text size (not used in file implementation).</param>
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

            writer.WriteLine(ReceiptText(formattedLine, maxLineCharacter));
        }
    }

    /// <summary>
    ///     Prints a barcode with specified parameters. This method creates a text representation of a barcode.
    /// </summary>
    /// <param name="type">The barcode type.</param>
    /// <param name="data">The barcode data.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="height">The barcode height.</param>
    /// <param name="alignment">The barcode alignment.</param>
    /// <param name="position">The barcode position.</param>
    public void PrintBarCode(int type, string data, int width, int height, int alignment, int position)
    {
        var lineChar = new string(Border.HorizontalLine, maxLineCharacter - 2);
        writer.WriteLine(ReceiptText($"{Border.TopLeft}{lineChar}{Border.TopRight}", maxLineCharacter));

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
        writer.WriteLine(ReceiptText(croppedBarcode, maxLineCharacter));

        var label = data.Length >= maxLineCharacter ? data[..maxLineCharacter] : data;
        var centeredLabel = label.PadLeft((maxChar + label.Length) / 2).PadRight(maxChar);
        writer.WriteLine(ReceiptText(ReceiptText(centeredLabel, maxChar), maxLineCharacter));

        writer.WriteLine(ReceiptText($"{Border.BottomLeft}{lineChar}{Border.BottomRight}", maxLineCharacter));
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
    ///     Prints an image representation to the file with a bordered frame and filename display.
    /// </summary>
    /// <param name="filePath">The path to the image file.</param>
    /// <param name="filename">The filename to display.</param>
    /// <param name="scaleMode">The scaling mode (not used in file implementation).</param>
    public void PrintImage(string filePath, string filename, int scaleMode)
    {
        var lineChar = new string(Border.HorizontalLine, maxLineCharacter - 2);
        writer.WriteLine(ReceiptText($"{Border.TopLeft}{lineChar}{Border.TopRight}", maxLineCharacter));

        var width = maxLineCharacter - 8;
        var lines = filename.SplitIntoLines(width);
        foreach (var line in lines)
        {
            var formatted = line.PadLeft((width + line.Length) / 2).PadRight(width);
            writer.WriteLine(ReceiptText(ReceiptText(formatted, width), maxLineCharacter));
        }

        writer.WriteLine(ReceiptText($"{Border.BottomLeft}{lineChar}{Border.BottomRight}", maxLineCharacter));
    }
}