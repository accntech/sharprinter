// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Specifies the text wrapping behavior for printed text.
/// </summary>
public enum TextWrap
{
    /// <summary>
    ///     No text wrapping.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Text wraps to the next line when it exceeds the line width.
    /// </summary>
    Wrap = 1
}