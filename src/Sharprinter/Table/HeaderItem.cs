// ReSharper disable once CheckNamespace
namespace Sharprinter;


/// <summary>
///     Represents a header item for table columns with label, maximum length, and alignment properties.
/// </summary>
public sealed class HeaderItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HeaderItem" /> class.
    /// </summary>
    /// <param name="label">The header label text.</param>
    /// <param name="maxLength">The maximum character length for the header column.</param>
    /// <param name="alignment">The text alignment for the header (default is <see cref="Alignment.Left" />).</param>
    public HeaderItem(string label, int maxLength, Alignment alignment = Alignment.Left)
    {
        Label = label;
        MaxLength = maxLength;
        Alignment = alignment;
    }

    /// <summary>
    ///     Gets the header label text.
    /// </summary>
    public string Label { get; }

    /// <summary>
    ///     Gets the maximum character length for the header column.
    /// </summary>
    public int MaxLength { get; }

    /// <summary>
    ///     Gets the text alignment for the header.
    /// </summary>
    public Alignment Alignment { get; }
}