// ReSharper disable once CheckNamespace
namespace Sharprinter;


/// <summary>
///     Represents a data item for table rows with label, maximum length, alignment, and text wrap properties.
/// </summary>
public sealed class LineItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LineItem" /> class.
    /// </summary>
    /// <param name="label">The item label text.</param>
    /// <param name="maxLength">The maximum character length for the item column.</param>
    /// <param name="alignment">The text alignment for the item (default is <see cref="Alignment.Left" />).</param>
    /// <param name="textWrap">Whether to enable text wrapping for the item (default is false).</param>
    public LineItem(string label, int maxLength, Alignment alignment = Alignment.Left, bool textWrap = false)
    {
        Label = label;
        MaxLength = maxLength;
        Alignment = alignment;
        TextWrap = textWrap;
    }

    /// <summary>
    ///     Gets the item label text.
    /// </summary>
    public string Label { get; }

    /// <summary>
    ///     Gets the maximum character length for the item column.
    /// </summary>
    public int MaxLength { get; }

    /// <summary>
    ///     Gets a value indicating whether text wrapping is enabled for the item.
    /// </summary>
    public bool TextWrap { get; }

    /// <summary>
    ///     Gets the text alignment for the item.
    /// </summary>
    public Alignment Alignment { get; }
}