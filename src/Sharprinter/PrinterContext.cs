using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sharprinter;

/// <summary>
///     Provides a context for building and executing print operations with method chaining support.
/// </summary>
public class PrinterContext(PrinterOptions options)
{
    /// <summary>
    ///     Gets the internal printer device handle.
    /// </summary>
    internal IntPtr Device { get; private set; }

    private readonly List<Action> _actions = [];

    /// <summary>
    ///     Add action to print text
    /// </summary>
    /// <param name="text">Text to be print</param>
    /// <param name="alignment">Text alignment in the paper default is <see cref="Alignment.Left" /></param>
    /// <param name="textSize">Text size</param>
    /// <returns>
    ///     <see cref="PrinterContext" />
    /// </returns>
    /// <exception cref="Exception">Throws an exception when printer returns error code</exception>
    public PrinterContext Text(string text, Alignment alignment = Alignment.Left, int textSize = 0)
    {
        _actions.Add(() =>
        {
            var result = Printer.PrintText(Device, text, (int)alignment, textSize);
            if (result != 0) throw new Exception($"Failed to print text. Error code: {result}");
        });
        return this;
    }

    /// <summary>
    ///     Add action to print text with new line
    /// </summary>
    /// <param name="text">Text to be print</param>
    /// <param name="textWrap">Wrap the text to next line</param>
    /// <param name="alignment">Text alignment in the paper default is <see cref="Alignment.Left" /></param>
    /// <param name="textSize">Text size</param>
    /// <returns>
    ///     <see cref="PrinterContext" />
    /// </returns>
    /// <exception cref="Exception">Throws an exception when printer returns error code</exception>
    public PrinterContext TextLine(string text, bool textWrap, Alignment alignment = Alignment.Left,
        int textSize = 0)
    {
        _actions.Add(() =>
        {
            if (textWrap) text = text.Wrap(options.MaxLineCharacter);

            var result = Printer.PrintText(Device, $"{text}\n", (int)alignment, textSize);
            if (result != 0) throw new Exception($"Failed to print text. Error code: {result}");
        });
        return this;
    }

    /// <summary>
    ///     Adds a separator line consisting of dashes.
    /// </summary>
    /// <returns>
    ///     <see cref="PrinterContext" /> for method chaining.
    /// </returns>
    public PrinterContext TextSeparator(char character = '-')
    {
        var separator = new string(character, options.MaxLineCharacter);
        return TextLine(separator, false);
    }

    /// <summary>
    ///     Add action to insert new line
    /// </summary>
    /// <param name="lines">Number of lines to be inserted</param>
    /// <returns>
    ///     <see cref="PrinterContext" />
    /// </returns>
    /// <exception cref="Exception">Throws an exception when printer returns error code</exception>
    public PrinterContext FeedLine(int lines = 1)
    {
        if (lines < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(lines), "Number of lines must be at least 1.");
        }

        _actions.Add(() =>
        {
            var result = Printer.PrintText(Device, new string('\n', lines), 0, 0);
            if (result != 0) throw new Exception($"Failed to feed line. Error code: {result}");
        });
        return this;
    }

    /// <summary>
    ///     Creates a new table builder for formatting tabular data.
    /// </summary>
    /// <returns>An <see cref="ITable" /> instance for building table content.</returns>
    public ITable Table()
    {
        return new Table(this, options.MaxLineCharacter);
    }

    /// <summary>
    ///     Add action to print image
    /// </summary>
    /// <param name="path">File path of the image</param>
    /// <returns>
    ///     <see cref="PrinterContext" />
    /// </returns>
    /// <exception cref="Exception">Throws an exception when printer returns error code</exception>
    public PrinterContext Image(string path)
    {
        _actions.Add(() =>
        {
            var result = Printer.PrintImage(Device, path, 0);
            if (result != 0) throw new Exception($"Failed to print image. Error code: {result}");
        });
        return this;
    }

    /// <summary>
    ///     Adds an action to print a barcode.
    /// </summary>
    /// <param name="barcode">The barcode data to be printed.</param>
    /// <param name="alignment">The alignment of the barcode (default is <see cref="Alignment.Left" />).</param>
    /// <returns>The current <see cref="PrinterContext" /> instance for method chaining.</returns>
    /// <exception cref="Exception">Throws an exception when printer returns error code</exception>
    public PrinterContext BarCode(string barcode, Alignment alignment = Alignment.Left)
    {
        _actions.Add(() =>
        {
            var result = Printer.PrintBarCode(Device, 73, barcode, 3, 100, (int)alignment, 2);
            if (result != 0) throw new Exception("Failed to print barcode");
        });
        return this;
    }

    /// <summary>
    ///     Opens the cash drawer asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public Task OpenDrawerAsync(CancellationToken cancellationToken = default)
    {
        return Task.Factory.StartNew(() =>
        {
            Device = Printer.InitPrinter("");
            var result = Printer.OpenPort(Device, $"{options.PortName}, {options.BaudRate}");

            if (result != 0) return;

            _ = Printer.OpenCashDrawer(Device, 0, 30, 255);
            _ = Printer.ClosePort(Device);
        }, cancellationToken);
    }

    /// <summary>
    ///     Start printing
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns></returns>
    /// <exception cref="Exception">Throws an exception when printer returns error code</exception>
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Task.Factory.StartNew(() =>
        {
            Device = Printer.InitPrinter("");
            var result = Printer.OpenPort(Device, $"{options.PortName}, {options.BaudRate}");

            if (result != 0) return;

            foreach (var action in _actions.TakeWhile(_ => !cancellationToken.IsCancellationRequested)) action();

            if (options.CutPaper) _ = Printer.CutPaperWithDistance(Device, 66);
            if (options.OpenDrawer) _ = Printer.OpenCashDrawer(Device, 0, 30, 255);

            _ = Printer.ClosePort(Device);
        }, cancellationToken);
    }

    /// <summary>
    ///     Adds table actions to the printer context for execution.
    /// </summary>
    /// <param name="actions">The list of table actions to add.</param>
    internal void AddTableActions(List<Action> actions)
    {
        _actions.AddRange(actions);
    }
}