﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sharprinter;

/// <summary>
///     Provides a context for building and executing print operations with method chaining support.
///     This class allows you to build complex print jobs by chaining multiple print operations
///     and then executing them as a single batch operation.
/// </summary>
/// <remarks>
///     The <see cref="PrinterContext" /> class provides a fluent API for building print jobs.
///     Each method returns the context instance, allowing for method chaining.
///     Print operations are queued and executed when <see cref="ExecuteAsync" /> is called.
/// </remarks>
public sealed class PrinterContext
{
    /// <summary>
    ///     Gets the printer configuration options including port settings, baud rate, and behavior flags.
    /// </summary>
    public PrinterOptions Options { get; }

    internal IPrinter Printer { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrinterContext" /> class with the specified options.
    /// </summary>
    /// <param name="options">The printer configuration options including port settings, baud rate, and behavior flags.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options" /> is null.</exception>
    public PrinterContext(PrinterOptions options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Printer = new Printer(Options.MaxLineCharacter);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PrinterContext" /> class with the specified printer and options.
    /// </summary>
    /// <param name="printer">The printer implementation to use for print operations.</param>
    /// <param name="options">The printer configuration options including port settings, baud rate, and behavior flags.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="printer" /> or <paramref name="options" /> is null.</exception>
    public PrinterContext(IPrinter printer, PrinterOptions options)
    {
        Printer = printer ?? throw new ArgumentNullException(nameof(printer));
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    ///     Adds a text line element to the print job with optional configuration.
    /// </summary>
    /// <param name="data">The text content to be printed as a line.</param>
    /// <param name="action">
    ///     An optional action to further configure the <see cref="IText" /> element (e.g., alignment, wrapping, size).
    /// </param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    public PrinterContext AddText(string data, Action<IText>? action = null)
    {
        var text = new Text(this, data);
        action?.Invoke(text);

        return text.Add();
    }

    /// <summary>
    ///     Adds a horizontal separator line to the print job.
    /// </summary>
    /// <param name="character">
    ///     The character to use for the separator line. Defaults to '─'.
    /// </param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    public PrinterContext AddSeparator(char character = '─')
    {
        if (character is '\n' or '\r')
        {
            throw new ArgumentException("Separator character cannot be a newline or carriage return.",
                nameof(character));
        }

        var separator = new string(character, Options.MaxLineCharacter);
        Printer.PrintText(separator);
        return this;
    }

    /// <summary>
    ///     Adds an action to insert one or more blank lines.
    /// </summary>
    /// <param name="lines">The number of blank lines to insert. Must be at least 1.</param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="lines" /> is less than 1.</exception>
    public PrinterContext FeedLine(int lines = 1)
    {
        if (lines < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(lines), "Number of lines must be at least 1.");
        }

        Printer.FeedLine(lines);
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
    public PrinterContext AddTable(Action<ITable> expression)
    {
        var table = new Table(this, Options.MaxLineCharacter);
        expression.Invoke(table);
        return this;
    }

    /// <summary>
    ///     Adds an image to the print queue with optional configuration.
    /// </summary>
    /// <param name="path">The file path of the image to be printed.</param>
    /// <param name="expression">
    ///     An optional action to configure the image using the <see cref="IImage" /> interface.
    ///     If not provided, default image settings will be used.
    /// </param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    public PrinterContext AddImage(string path, Action<IImage>? expression = null)
    {
        var image = new Image(this, path);
        expression?.Invoke(image);

        return image.Add();
    }

    /// <summary>
    ///     Adds a barcode to the print queue with optional configuration.
    /// </summary>
    /// <param name="data">The data to encode in the barcode.</param>
    /// <param name="expression">
    ///     An optional action to configure the barcode using the <see cref="IBarcode" /> interface.
    ///     If not provided, default barcode settings will be used.
    /// </param>
    /// <returns>
    ///     The current <see cref="PrinterContext" /> instance for method chaining.
    /// </returns>
    public PrinterContext AddBarcode(string data, Action<IBarcode>? expression = null)
    {
        var barcode = new Barcode(this, data);
        expression?.Invoke(barcode);

        return barcode.Add();
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
            Printer.OpenPort($"{Options.PortName}, {Options.BaudRate}");
            Printer.OpenCashDrawer();
            Printer.ClosePort();
        }, cancellationToken);
    }

    /// <summary>
    ///     Executes all queued print operations asynchronously.
    /// </summary>
    /// <param name="token">A token to monitor for cancellation requests.</param>
    public Task ExecuteAsync(CancellationToken token = default)
    {
        return Task.Factory.StartNew(() =>
        {
            Printer.Initialize();
            Printer.OpenPort($"{Options.PortName}, {Options.BaudRate}");

            Printer.Print(token);

            if (Options.CutPaper) Printer.CutPaperWithDistance(66);
            if (Options.OpenDrawer) Printer.OpenCashDrawer();

            Printer.Release();
            Printer.ClosePort();
        }, token);
    }
}