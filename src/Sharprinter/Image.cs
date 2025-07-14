namespace Sharprinter;

/// <summary>
///     Represents an image element that can be configured and added to a printer context.
/// </summary>
public interface IImage
{
    /// <summary>
    ///     Sets the scale mode for the image.
    /// </summary>
    /// <param name="scaleMode">The scale mode to apply to the image (e.g., Normal, Fit, Stretch).</param>
    /// <returns>The current image element for method chaining.</returns>
    IImage ScaleMode(ScaleMode scaleMode);

    /// <summary>
    ///     Sets the filename for the image.
    /// </summary>
    /// <param name="filename">Use as a image placeholder other than the actual physical printer.</param>
    /// <returns>The current image element for method chaining.</returns>
    IImage Filename(string filename);
}

internal sealed class Image(PrinterContext context, string path) : IImage
{
    private ScaleMode _scaleMode = Sharprinter.ScaleMode.Normal;
    private string _filename = string.Empty;

    public IImage ScaleMode(ScaleMode scaleMode)
    {
        _scaleMode = scaleMode;
        return this;
    }

    public IImage Filename(string filename)
    {
        _filename = filename;
        return this;
    }

    internal PrinterContext Add()
    {
        context.AddAction(() => context.Printer.PrintImage(path, _filename, _scaleMode));
        return context;
    }
}