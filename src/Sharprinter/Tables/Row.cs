using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
///     Represents a row in a table, providing methods to add items.
/// </summary>
public interface IRow
{
    /// <summary>
    ///     Adds a row item with the specified data and an optional configuration expression.
    /// </summary>
    /// <param name="data">The data to be added to the row item.</param>
    /// <param name="expression">An optional action to further configure the row item.</param>
    /// <returns>The current <see cref="IRow" /> instance for chaining.</returns>
    IRow AddRowItem(string data, Action<IRowItem>? expression = null);

    /// <summary>
    ///     Adds a row item with the specified data, maximum length, and an optional configuration expression.
    /// </summary>
    /// <param name="data">The data to be added to the row item.</param>
    /// <param name="maxLength">The maximum length of the row item.</param>
    /// <param name="expression">An optional action to further configure the row item.</param>
    /// <returns>The current <see cref="IRow" /> instance for chaining.</returns>
    IRow AddRowItem(string data, int maxLength, Action<IRowItem>? expression = null);
}

internal sealed class Row(int maxLineCharacter) : IRow
{
    private readonly List<IRowItem> _items = [];

    public IRow AddRowItem(string data, int maxLength, Action<IRowItem>? expression = null)
    {
        var item = new RowItem(data, maxLength);
        expression?.Invoke(item);

        _items.Add(item);
        RealignText();

        return this;
    }

    private void RealignText()
    {
        var noWidthCount = _items.Count(x => x is RowItem { MaxLength: null });
        var staticMaxLength = _items.Sum(x => x is RowItem i ? i.MaxLength ?? 0 : 0);
        var sharedMaxLength = 0;

        if (noWidthCount > 0)
        {
            sharedMaxLength = (maxLineCharacter - staticMaxLength) / noWidthCount;
        }

        foreach (var i in _items)
        {
            i.UpdateLines(i.MaxLength ?? sharedMaxLength);
        }

        var minRow = _items.Max(x => x.LineItems.Count);
        foreach (var i in _items)
        {
            i.AlignVertical(minRow, i.MaxLength ?? sharedMaxLength);
        }
    }

    public IRow AddRowItem(string data, Action<IRowItem>? expression = null)
    {
        var item = new RowItem(data);
        expression?.Invoke(item);

        _items.Add(item);
        RealignText();

        return this;
    }

    internal List<string> GetLineItems()
    {
        var maxRows = _items.Max(x => x.LineItems.Count);
        var index = 0;

        var lineItems = new List<string>();
        while (index < maxRows)
        {
            var groupedLines = _items.Select(x => x.LineItems.ElementAtOrDefault(index) ?? string.Empty).ToArray();
            lineItems.Add(string.Join("", groupedLines));
            index++;
        }

        return lineItems;
    }
}