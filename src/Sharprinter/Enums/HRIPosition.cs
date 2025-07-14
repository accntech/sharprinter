// ReSharper disable once CheckNamespace

namespace Sharprinter;

/// <summary>
///     Specifies the position of the Human Readable Interpretation (HRI) for barcodes.
/// </summary>
// ReSharper disable once InconsistentNaming
public enum HRIPosition
{
    /// <summary>
    ///     No HRI is printed.
    /// </summary>
    None = 0,

    /// <summary>
    ///     HRI is printed above the barcode.
    /// </summary>
    Top = 1,

    /// <summary>
    ///     HRI is printed below the barcode.
    /// </summary>
    Below = 2,

    /// <summary>
    ///     HRI is printed both above and below the barcode.
    /// </summary>
    AboveAndBelow = 3
}