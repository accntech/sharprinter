namespace Sharprinter;

/// <summary>
///     Represents a barcode element that can be configured and added to a printer context.
/// </summary>
public interface IBarcode
{
    /// <summary>
    ///     Sets the height for the barcode.
    /// </summary>
    /// <param name="height">The height of the barcode in printer units.</param>
    /// <returns>The current barcode element for method chaining.</returns>
    IBarcode Height(int height);

    /// <summary>
    ///     Sets the width for the barcode.
    /// </summary>
    /// <param name="width">The width of the barcode.</param>
    /// <returns>The current barcode element for method chaining.</returns>
    IBarcode Width(BarcodeWidth width);

    /// <summary>
    ///     Sets the horizontal alignment for the barcode.
    /// </summary>
    /// <param name="alignment">The horizontal alignment to apply.</param>
    /// <returns>The current barcode element for method chaining.</returns>
    IBarcode Alignment(HorizontalAlignment alignment);

    /// <summary>
    ///     Sets the HRI (Human Readable Interpretation) position for the barcode.
    /// </summary>
    /// <param name="position">The HRI position to apply.</param>
    /// <returns>The current barcode element for method chaining.</returns>
    IBarcode Position(HRIPosition position);
}

internal sealed class Barcode(PrinterContext context, string barcode) : IBarcode
{
    private int _height = 100;
    private BarcodeWidth _width = BarcodeWidth.Large;
    private HorizontalAlignment _alignment = HorizontalAlignment.Center;
    private HRIPosition _position = HRIPosition.Below;

    public IBarcode Height(int height)
    {
        _height = height;
        return this;
    }

    public IBarcode Width(BarcodeWidth width)
    {
        _width = width;
        return this;
    }

    public IBarcode Alignment(HorizontalAlignment alignment)
    {
        _alignment = alignment;
        return this;
    }

    public IBarcode Position(HRIPosition position)
    {
        _position = position;
        return this;
    }

    internal PrinterContext Add()
    {
        context.Printer.PrintBarCode(barcode, _height, _width, _alignment, _position);
        return context;
    }
}