# Sharprinter

A modern, fluent C# wrapper for thermal printer SDK operations for Windows, designed for POS (Point of Sale) systems and
receipt printing.

## 🚀 Features

- **Text Printing** - Print text with alignment, sizing, and wrapping
- **Table Support** - Create formatted tables with headers and rows
- **Image Printing** - Print images from file paths
- **Barcode Support** - Generate and print barcodes
- **Cash Drawer Control** - Open cash drawers programmatically
- **Paper Cutting** - Automatic or manual paper cutting
- **Fluent API Design** - Chain operations for clean, readable code

## 📦 Installation

### NuGet Package

```bash
dotnet add package Sharprinter
```

### Package Manager

```bash
Install-Package Sharprinter
```

## 🔧 Quick Start

### Basic Text Printing

```csharp
using Sharprinter;

var options = new PrinterOptions
{
    PortName = "COM3",
    BaudRate = 9600,
    MaxLineCharacter = 48,
    CutPaper = true
};

var context = new PrinterContext(options);

await context
    .TextLine("Welcome to our store!", false, Alignment.Center, 1)
    .TextLine("Thank you for your purchase!", true)
    .FeedLine(2)
    .ExecuteAsync();
```

### Table Printing

```csharp
await context
    .Table()
    .AddHeader([
        new HeaderItem("Item", 20),
        new HeaderItem("Qty", 8, Alignment.Center),
        new HeaderItem("Price", 15, Alignment.Right)
    ])
    .AddLine([
        new LineItem("Coffee", 20),
        new LineItem("2", 8, Alignment.Center),
        new LineItem("$5.50", 15, Alignment.Right)
    ])
    .AddLine([
        new LineItem("Sandwich", 20),
        new LineItem("1", 8, Alignment.Center),
        new LineItem("$8.99", 15, Alignment.Right)
    ])
    .AddRowSeparator()
    .AddLine("Total", "$14.49")
    .Create()
    .ExecuteAsync();
```

### Receipt Example

```csharp
var receiptContext = new PrinterContext(options);

await receiptContext
    .TextLine("RECEIPT", false, Alignment.Center, 2)
    .TextLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm}", false)
    .TextLine($"Receipt #: {Guid.NewGuid().ToString()[..8].ToUpper()}", false)
    .TextSeparator()
    .Table()
    .AddHeader([
        new HeaderItem("Item", 25),
        new HeaderItem("Price", 15, Alignment.Right)
    ])
    .AddLine("Espresso", "$3.50")
    .AddLine("Croissant", "$2.75")
    .AddLine("Tax", "$0.50")
    .AddRowSeparator()
    .AddLine("TOTAL", "$6.75", 10)
    .Create()
    .FeedLine(1)
    .TextLine("Thank you for your business!", false, Alignment.Center)
    .BarCode("1234567890123", Alignment.Center)
    .ExecuteAsync();
```

## 📚 API Reference

### PrinterContext

The main class for building print operations using a fluent interface.

#### Methods

| Method                                   | Description                                 |
|------------------------------------------|---------------------------------------------|
| `Text(string, Alignment, int)`           | Add text without newline                    |
| `TextLine(string, bool, Alignment, int)` | Add text with newline and optional wrapping |
| `TextSeparator(char)`                    | Add a separator line                        |
| `FeedLine(int)`                          | Insert blank lines                          |
| `Table()`                                | Create a table builder                      |
| `Image(string)`                          | Add image from file path                    |
| `BarCode(string, Alignment)`             | Add barcode                                 |
| `ExecuteAsync(CancellationToken)`        | Execute all queued operations               |

### PrinterOptions

Configuration class for printer settings.

```csharp
public class PrinterOptions
{
    public int MaxLineCharacter { get; set; }    // Characters per line
    public string PortName { get; set; }         // COM port (e.g., "COM3")
    public int BaudRate { get; set; } = 9600;    // Communication speed
    public bool OpenDrawer { get; set; }         // Open cash drawer after printing
    public bool CutPaper { get; set; }           // Cut paper after printing
}
```

### Table API

Create formatted tables with headers and data rows.

```csharp
context.Table()
    .AddHeader(HeaderItem[])           // Add table headers
    .AddLine(LineItem[])               // Add data row
    .AddLine(string, string, int)      // Add label-value pair
    .AddLine(string)                   // Add single column text
    .AddEmptyLine()                    // Add blank row
    .AddRowSeparator()                 // Add separator line
    .Create()                          // Build table and return context
```

### Alignment Options

```csharp
public enum Alignment
{
    Left = 0,
    Center = 1,
    Right = 2,
    Justify = 3
}
```

## 🛠️ Advanced Usage

### Custom Table Formatting

```csharp
await context
    .Table()
    .AddHeader([
        new HeaderItem("Product", 25, Alignment.Left),
        new HeaderItem("Qty", 8, Alignment.Center),
        new HeaderItem("Unit Price", 12, Alignment.Right),
        new HeaderItem("Total", 12, Alignment.Right)
    ])
    .AddLine([
        new LineItem("Premium Coffee Beans", 25),
        new LineItem("2", 8, Alignment.Center),
        new LineItem("$12.99", 12, Alignment.Right),
        new LineItem("$25.98", 12, Alignment.Right)
    ])
    .Create()
    .ExecuteAsync();
```

### Text Wrapping

```csharp
await context
    .TextLine("This is a very long line that will be wrapped automatically based on the MaxLineCharacter setting",
              textWrap: true)
    .ExecuteAsync();
```

### Async Operations with Cancellation

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

try
{
    await context
        .TextLine("Processing...", false)
        .ExecuteAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Print operation was cancelled");
}
```

## ⚙️ Requirements

- **Runtime**: .NET Standard 2.1 or .NET 9.0+
- **Platform**: Windows (requires `printer.sdk.dll`)
- **Hardware**: Compatible thermal printer with serial/bluetooth connection

## 🔧 Configuration

### Common Port Settings

- **Serial**: `COM1`, `COM2`, `COM3`, etc.
- **USB**: Check device manager for correct port
- **Baud Rate**: Usually `9600` or `115200`

### Printer Setup

1. Install printer drivers
2. Configure port settings in Windows Device Manager
3. Test connection with manufacturer's software
4. Set correct `PortName` and `BaudRate` in `PrinterOptions`

## 🤝 Contributing

Contributions are welcome! Please feel free to submit pull requests or create issues for bugs and feature requests.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
