namespace Sharprinter;

/// <summary>
///     Represents a text element that can be configured and added to a printer context.
/// </summary>
public interface IText
{
    /// <summary>
    ///     Enables text wrapping for the current text element.
    /// </summary>
    /// <returns>The current text element for method chaining.</returns>
    IText Wrap();

    /// <summary>
    ///     Sets the horizontal alignment for the current text element.
    /// </summary>
    /// <param name="alignment">The horizontal alignment to apply.</param>
    /// <returns>The current text element for method chaining.</returns>
    IText Alignment(HorizontalAlignment alignment);

    /// <summary>
    ///     Sets the text size for the current text element.
    /// </summary>
    /// <param name="size">The text size to apply.</param>
    /// <returns>The current text element for method chaining.</returns>
    IText TextSize(TextSize size);
}

internal sealed class Text(PrinterContext context, string text) : IText
{
    private TextWrap _textWrap = TextWrap.None;
    private HorizontalAlignment _alignment = HorizontalAlignment.Left;
    private TextSize _textSize = Sharprinter.TextSize.Normal;

    public IText Wrap()
    {
        _textWrap = TextWrap.Wrap;
        return this;
    }

    public IText Alignment(HorizontalAlignment alignment)
    {
        _alignment = alignment;
        return this;
    }

    public IText TextSize(TextSize size)
    {
        _textSize = size;
        return this;
    }

    internal PrinterContext Add()
    {
        context.Printer.PrintText(text, _textWrap, _alignment, _textSize);
        return context;
    }
}