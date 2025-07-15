using System;
using System.Linq;
using System.Text;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     A console-based implementation of the IPrinter interface that simulates printer operations
///     by outputting formatted text to the console with visual borders and formatting.
/// </summary>
/// <param name="maxLineCharacter">The maximum number of characters per line for the receipt output.</param>
public sealed class ConsolePrinter(int maxLineCharacter) : PrinterBase, IPrinter
{
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
    }

    /// <summary>
    ///     Closes the console port and displays a bottom border for the receipt output.
    /// </summary>
    public void ClosePort()
    {
        Console.WriteLine("Console port closed.");
    }

    /// <summary>
    ///     Executes a collection of print actions.
    /// </summary>
    /// <param name="token">A cancellation token to observe while executing the actions.</param>
    public void Print(CancellationToken token)
    {
        if (PrintActions.Count == 0) return;

        var top = new string(Border.HorizontalLine, maxLineCharacter + Border.Padding - 2);
        Console.WriteLine($"{Border.TopLeft}{top}{Border.TopRight}");

        foreach (var action in PrintActions.TakeWhile(_ => !token.IsCancellationRequested)) action();

        var bottom = new string(Border.HorizontalLine, maxLineCharacter + Border.Padding - 2);
        Console.WriteLine($"{Border.BottomLeft}{bottom}{Border.BottomRight}");
        Console.WriteLine();
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
    /// <param name="distance">The distance for the paper cut operation (not used in console implementation).</param>
    public void CutPaperWithDistance(int distance)
    {
        Console.WriteLine($"Paper cut with distance {distance} requested.");
        Console.WriteLine();
    }

    /// <summary>
    ///     Feeds the specified number of empty lines to simulate paper feed.
    /// </summary>
    /// <param name="lines">The number of lines to feed.</param>
    public void FeedLine(int lines = 1)
    {
        AddToPrintQueue(() =>
        {
            var line = 0;
            while (line < lines)
            {
                Console.WriteLine(ReceiptText(string.Empty, maxLineCharacter));
                line++;
            }
        });
    }

    /// <summary>
    ///     Simulates opening a cash drawer by outputting a confirmation message.
    /// </summary>
    /// <param name="pinMode">The pin mode for the cash drawer operation (not used in console implementation).</param>
    /// <param name="onTime">The on time duration for the cash drawer operation (not used in console implementation).</param>
    /// <param name="ofTime">The off time duration for the cash drawer operation (not used in console implementation).</param>
    public void OpenCashDrawer(int pinMode = 0, int onTime = 30, int ofTime = 255)
    {
        Console.WriteLine("Open cash drawer requested");
        Console.WriteLine();
    }

    private static string ReceiptText(string text, int maxChar)
    {
        var paddedLeft = $"   {text}";
        var paddedRight = paddedLeft.PadRight(maxChar + Border.Padding - 2);

        return $"{Border.VerticalLine}{paddedRight}{Border.VerticalLine}";
    }

    /// <summary>
    ///     Prints text with specified alignment and text size to the console with visual borders.
    ///     Each line of text is printed on a separate line with a newline character.
    ///     Note: The textWrap parameter controls whether text is wrapped to multiple lines.
    /// </summary>
    /// <param name="data">The text data to print.</param>
    /// <param name="textWrap">Indicates whether text wrapping is enabled.</param>
    /// <param name="alignment">The text alignment (0=Left, 1=Center, 2=Right).</param>
    /// <param name="textSize">The text size (not used in console implementation).</param>
    public void PrintText(string data, TextWrap textWrap, HorizontalAlignment alignment, TextSize textSize)
    {
        AddToPrintQueue(() =>
        {
            if (textWrap == TextWrap.None)
            {
                var trimmed = data.Length > maxLineCharacter
                    ? data[..maxLineCharacter]
                    : data;

                var line = alignment switch
                {
                    HorizontalAlignment.Left => trimmed, // Left alignment
                    HorizontalAlignment.Center => trimmed.PadLeft((maxLineCharacter + trimmed.Length) / 2)
                        .PadRight(maxLineCharacter), // Center alignment
                    HorizontalAlignment.Right => trimmed.PadRight(maxLineCharacter), // Right alignment
                    _ => trimmed
                };
                Console.WriteLine(ReceiptText(line, maxLineCharacter));
                return;
            }

            var lines = data.SplitIntoLines(maxLineCharacter);

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

                Console.WriteLine(ReceiptText(formattedLine, maxLineCharacter));
            }
        });
    }

    /// <summary>
    ///     Prints a barcode with specified parameters. This method is not implemented in the console version.
    /// </summary>
    /// <param name="data">The barcode data.</param>
    /// <param name="width">The barcode width.</param>
    /// <param name="height">The barcode height.</param>
    /// <param name="alignment">The barcode alignment.</param>
    /// <param name="position">The barcode position.</param>
    /// <exception cref="NotImplementedException">Thrown because barcode printing is not implemented in the console version.</exception>
    public void PrintBarCode(
        string data,
        int height,
        BarcodeWidth width = BarcodeWidth.Large,
        HorizontalAlignment alignment = HorizontalAlignment.Left,
        HRIPosition position = HRIPosition.None)
    {
        AddToPrintQueue(() =>
        {
            var lineChar = new string(Border.HorizontalLine, maxLineCharacter - 2);
            Console.WriteLine(ReceiptText($"{Border.TopLeft}{lineChar}{Border.TopRight}", maxLineCharacter));

            var maxChar = maxLineCharacter - 8;
            var remainingData = data.Length >= maxChar ? data[..maxChar] : data;
            var barcode = GenerateDummyBarcode(remainingData);

            var formatted = alignment switch
            {
                HorizontalAlignment.Left => barcode, // Left alignment
                HorizontalAlignment.Center => barcode.PadLeft((maxChar + barcode.Length) / 2)
                    .PadRight(maxChar), // Center alignment
                HorizontalAlignment.Right => barcode.PadRight(maxChar), // Right alignment
                _ => barcode
            };

            var croppedBarcode = formatted.Length > maxLineCharacter
                ? formatted[..maxLineCharacter]
                : formatted;
            Console.WriteLine(ReceiptText(croppedBarcode, maxLineCharacter));

            var label = data.Length >= maxLineCharacter ? data[..maxLineCharacter] : data;
            var centeredLabel = label.PadLeft((maxChar + label.Length) / 2).PadRight(maxChar);
            Console.WriteLine(ReceiptText(ReceiptText(centeredLabel, maxChar), maxLineCharacter));

            Console.WriteLine(ReceiptText($"{Border.BottomLeft}{lineChar}{Border.BottomRight}", maxLineCharacter));
        });
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
    public void PrintImage(string filePath, string filename, ScaleMode scaleMode)
    {
        AddToPrintQueue(() =>
        {
            var lineChar = new string(Border.HorizontalLine, maxLineCharacter - 2);
            Console.WriteLine(ReceiptText($"{Border.TopLeft}{lineChar}{Border.TopRight}", maxLineCharacter));

            var width = maxLineCharacter - 8;
            var lines = filename.SplitIntoLines(width);
            foreach (var line in lines)
            {
                var formatted = line.PadLeft((width + line.Length) / 2).PadRight(width);
                Console.WriteLine(ReceiptText(ReceiptText(formatted, width), maxLineCharacter));
            }

            Console.WriteLine(ReceiptText($"{Border.BottomLeft}{lineChar}{Border.BottomRight}", maxLineCharacter));
        });
    }
}