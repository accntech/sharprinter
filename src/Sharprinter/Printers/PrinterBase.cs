using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Sharprinter;

public class PrinterBase
{
    protected readonly List<Action> PrintActions = [];

    protected void AddToPrintQueue(Action action)
    {
        PrintActions.Add(action);
    }
}