using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharprinter;

/// <summary>
///     Provides a context for building and executing print operations with method chaining support.
///     This class allows you to build complex print jobs by chaining multiple print operations
///     and then executing them as a single batch operation.
/// </summary>
/// <remarks>
///     The <see cref="PrinterContext"/> class provides a fluent API for building print jobs.
///     Each method returns the context instance, allowing for method chaining.
///     Print operations are queued and executed when <see cref="ExecuteAsync"/> is called.
/// </remarks>
public class PrinterContext
{
    internal IPrinter Printer { get; }

    private readonly List<Action> _actions = [];
    private readonly PrinterOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrinterContext" /> class with the specified options.
    /// </summary>
    /// <param name="options">The printer configuration options including port settings, baud rate, and behavior flags.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
    public PrinterContext(PrinterOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        Printer = new Printer();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrinterContext" /> class with the specified printer and options.
    /// </summary>
    /// <param name="printer">The printer implementation to use for print operations.</param>
    /// <param name="options">The printer configuration options including port settings, baud rate, and behavior flags.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="printer"/> or <paramref name="options"/> is null.</exception>
    public PrinterContext(IPrinter printer, PrinterOptions options)
    {
        Printer = printer ?? throw new ArgumentNullException(nameof(printer));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    ///     Adds an action to print text at the current position.
    /// </summary>
    /// <param name="text">The text content to be printed.</param>
    /// <param name="textWrap">Whether to wrap the text to the next line if it exceeds the paper width.</param>
    /// <param name="horizontalAlignment">The horizontal alignment of the text on the paper. Default is <see cref="HorizontalAlignment.Left" />.</param>
    /// <param name="textSize">The size of the text. Use 0 for default size.</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="text"/> is null or empty.</exception>
    public PrinterContext Text(string text, bool textWrap, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, int textSize = 0)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));

        _actions.Add(() => Printer.PrintText(text, textWrap, (int)horizontalAlignment, textSize));
        return this;
    }

    /// <summary>
    ///     Adds an action to print text followed by a new line.
    /// </summary>
    /// <param name="text">The text content to be printed.</param>
    /// <param name="textWrap">Whether to wrap the text to the next line if it exceeds the paper width.</param>
    /// <param name="horizontalAlignment">The horizontal alignment of the text on the paper. Default is <see cref="HorizontalAlignment.Left" />.</param>
    /// <param name="textSize">The size of the text. Use 0 for default size.</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="text"/> is null or empty.</exception>
    public PrinterContext TextLine(string text, bool textWrap, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left,
        int textSize = 0)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Text cannot be null or empty.", nameof(text));

        _actions.Add(() =>
        {
            Printer.PrintTextLine($"{text}", textWrap, (int)horizontalAlignment, textSize);
        });
        return this;
    }

    /// <summary>
    ///     Adds a separator line consisting of repeated characters.
    /// </summary>
    /// <param name="character">The character to use for the separator line. Default is '─' (horizontal line).</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <remarks>
    ///     The separator line will span the full width of the paper as defined by <see cref="PrinterOptions.MaxLineCharacter"/>.
    /// </remarks>
    public PrinterContext TextSeparator(char character = '─')
    {
        var separator = new string(character, _options.MaxLineCharacter);
        return TextLine(separator, false);
    }

    /// <summary>
    ///     Adds an action to insert one or more blank lines.
    /// </summary>
    /// <param name="lines">The number of blank lines to insert. Must be at least 1.</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="lines"/> is less than 1.</exception>
    public PrinterContext FeedLine(int lines = 1)
    {
        if (lines < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(lines), "Number of lines must be at least 1.");
        }

        _actions.Add(() => Printer.FeedLine(lines));
        return this;
    }

    /// <summary>
    ///     Creates a new table builder for formatting tabular data.
    /// </summary>
    /// <returns>
    ///     An <see cref="ITable" /> instance for building table content with the configured paper width.
    /// </returns>
    /// <remarks>
    ///     The table will be configured to use the maximum line character count from the printer options
    ///     to ensure proper formatting within the paper width.
    /// </remarks>
    public ITable Table()
    {
        return new Table(this, _options.MaxLineCharacter);
    }

    /// <summary>
    ///     Adds an action to print an image from a file.
    /// </summary>
    /// <param name="path">The file path of the image to be printed.</param>
    /// <param name="filename">Optional filename for the image. If not provided, the filename from the path will be used.</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is null, empty, or invalid.</exception>
    /// <exception cref="System.IO.FileNotFoundException">Thrown when the image file does not exist.</exception>
    public PrinterContext Image(string path, string filename = "")
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Image path cannot be null or empty.", nameof(path));

        _actions.Add(() => { Printer.PrintImage(path, filename, 0); });
        return this;
    }

    /// <summary>
    ///     Adds an action to print a barcode.
    /// </summary>
    /// <param name="barcode">The barcode data to be printed.</param>
    /// <param name="horizontalAlignment">The horizontal alignment of the barcode on the paper. Default is <see cref="HorizontalAlignment.Left" />.</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="barcode"/> is null or empty.</exception>
    /// <remarks>
    ///     The barcode will be printed using Code 128 format with default height and width settings.
    /// </remarks>
    public PrinterContext BarCode(string barcode, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left)
    {
        if (string.IsNullOrEmpty(barcode))
            throw new ArgumentException("Barcode data cannot be null or empty.", nameof(barcode));

        _actions.Add(() => Printer.PrintBarCode(73, barcode, 3, 100, (int)horizontalAlignment, 2));
        return this;
    }

    /// <summary>
    ///     Opens the cash drawer asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    ///     The task completes when the cash drawer has been opened.
    /// </returns>
    /// <remarks>
    ///     This method initializes the printer, opens the configured port, sends the cash drawer
    ///     open command, and then closes the port. The operation is performed asynchronously
    ///     to avoid blocking the calling thread.
    /// </remarks>
    public Task OpenDrawerAsync(CancellationToken cancellationToken = default)
    {
        return Task.Factory.StartNew(() =>
        {
            Printer.Initialize();
            Printer.OpenPort($"{_options.PortName}, {_options.BaudRate}");
            Printer.OpenCashDrawer(0, 30, 255);
            Printer.ClosePort();
        }, cancellationToken);
    }

    /// <summary>
    ///     Executes all queued print operations asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Task.Factory.StartNew(() =>
        {
            Printer.Initialize();
            Printer.OpenPort($"{_options.PortName}, {_options.BaudRate}");

            foreach (var action in _actions.TakeWhile(_ => !cancellationToken.IsCancellationRequested)) action();

            if (_options.CutPaper) Printer.CutPaperWithDistance(66);
            if (_options.OpenDrawer) Printer.OpenCashDrawer(0, 30, 255);

            Printer.ClosePort();
        }, cancellationToken);
    }

    /// <summary>
    ///     Adds table actions to the printer context for execution.
    /// </summary>
    /// <param name="actions">The list of table actions to add to the print queue.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="actions"/> is null.</exception>
    internal void AddTableActions(List<Action> actions)
    {
        _actions.AddRange(actions ?? throw new ArgumentNullException(nameof(actions)));
    }
}