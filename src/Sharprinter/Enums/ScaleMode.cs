// ReSharper disable once CheckNamespace

namespace Sharprinter;

/// <summary>
///     Specifies the scaling mode for printing.
/// </summary>
public enum ScaleMode
{
    /// <summary>
    ///     Normal scaling (1:1).
    /// </summary>
    Normal = 0,

    /// <summary>
    ///     Double width scaling.
    /// </summary>
    DoubleWidth = 1,

    /// <summary>
    ///     Double height scaling.
    /// </summary>
    DoubleHeight = 2,

    /// <summary>
    ///     Double width and height scaling.
    /// </summary>
    DoubleWidthAndHeight = 3
}