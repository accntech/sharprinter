using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

/// <summary>
/// Provides a base class for printer implementations, managing a queue of print actions.
/// </summary>
public class PrinterBase
{
    /// <summary>
    /// The list of actions representing the print queue.
    /// </summary>
    protected List<Action> PrintActions { get; } = [];

    /// <summary>
    /// Adds an action to the print queue.
    /// </summary>
    /// <param name="action">The action to add to the print queue.</param>
    protected void AddToPrintQueue(Action action)
    {
        PrintActions.Add(action);
    }
}