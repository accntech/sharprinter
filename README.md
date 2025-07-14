# Sharprinter

A modern, fluent C# wrapper for thermal printer SDK operations for Windows, designed for POS (Point of Sale) systems and
receipt printing.

[![NuGet](https://img.shields.io/nuget/v/Sharprinter.svg)](https://www.nuget.org/packages/Sharprinter)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sharprinter)](https://www.nuget.org/packages/Sharprinter)
[![GitHub stars](https://img.shields.io/github/stars/accntech/sharprinter)](https://github.com/accntech/sharprinter/stargazers)

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
    PortName = "COM1",
    BaudRate = 9600,
    MaxLineCharacter = 32,
    CutPaper = true
};

var context = new PrinterContext(options);

await context
    .AddText("Welcome to our store!", x => x.Alignment(HorizontalAlignment.Center))
    .AddText("Thank you for your purchase!", x => x.Alignment(HorizontalAlignment.Center))
    .FeedLine(2)
    .ExecuteAsync();
```

### Table Printing

```csharp
await context
    .AddTable(t => t
        .AddSeparator()
        .AddRow(r => r
        .AddRowItem("Item", i => i
            .HorizontalAlignment(HorizontalAlignment.Left)
            .VerticalAlignment(VerticalAlignment.Center))
        .AddRowItem("Qty", i => i
            .HorizontalAlignment(HorizontalAlignment.Center)
            .VerticalAlignment(VerticalAlignment.Center))
        .AddRowItem("Price", 8, i => i
            .HorizontalAlignment(HorizontalAlignment.Right)
            .VerticalAlignment(VerticalAlignment.Center)))
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("Coffee")
            .AddRowItem("2", 1)
            .AddRowItem(" x ", 3)
            .AddRowItem("2.75", 5)
            .AddRowItem("$5.50", 8, i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("Total", 10)
            .AddRowItem("$11.00", i => i.HorizontalAlignment(HorizontalAlignment.Right))))
    .ExecuteAsync();
```

### Complete Receipt Example

```csharp
var receiptContext = new PrinterContext(options);

await receiptContext
    .AddImage(@".\Assets\logo.jpg", x => x.Filename("Store Logo"))
    .AddText("123 Main St. Springfield, USA", x => x.Alignment(HorizontalAlignment.Center))
    .AddText("VATREG 123456789000", x => x.Alignment(HorizontalAlignment.Center))
    .FeedLine()
    .AddText("RECEIPT", x => x.Alignment(HorizontalAlignment.Center).TextSize(TextSize.DoubleHeight))
    .FeedLine()
    .AddText("MIN: 1234567890")
    .AddText("SN: 1A2B3C4D5E6F7G8H9")
    .FeedLine()
    .AddText("Order No: 123456")
    .AddText($"Date: {DateTime.Now:MM/dd/yyyy HH:mm:ss}")
    .AddSeparator()
    .AddText("Cashier: John Doe")
    .AddText("Customer: Jane Smith")
    .AddTable(t => t
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("Description", i => i
                .TextWrap()
                .HorizontalAlignment(HorizontalAlignment.Left)
                .VerticalAlignment(VerticalAlignment.Center))
            .AddRowItem("Amount", 12, i => i
                .HorizontalAlignment(HorizontalAlignment.Center)
                .VerticalAlignment(VerticalAlignment.Center)))
        .AddSeparator()
        .AddRow(r => r.AddRowItem("Espresso"))
        .AddRow(r => r
            .AddRowItem(" 1", 4)
            .AddRowItem(" x ", 3)
            .AddRowItem("$3.50", 10)
            .AddRowItem("$3.50", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .AddRow(r => r.AddRowItem("Croissant"))
        .AddRow(r => r
            .AddRowItem(" 1", 4)
            .AddRowItem(" x ", 3)
            .AddRowItem("$2.75", 10)
            .AddRowItem("$2.75", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("Subtotal", 8)
            .AddRowItem("$6.25", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .AddRow(r => r
            .AddRowItem("Tax", 6)
            .AddRowItem("$0.50", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("TOTAL", 10)
            .AddRowItem("$6.75", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .FeedLine()
        .AddSeparator()
        .FeedLine())
    .AddText("Thank you for your business!", x => x.Alignment(HorizontalAlignment.Center))
    .FeedLine()
    .AddBarcode("1234567890123", x => x.Height(75).Alignment(HorizontalAlignment.Center))
    .FeedLine(2)
    .ExecuteAsync();
```

## 📚 API Reference

### PrinterContext

The main class for building print operations using a fluent interface.

#### Core Methods

| Method                                       | Description                         |
| -------------------------------------------- | ----------------------------------- |
| `AddText(string, Action<TextOptions>)`       | Add text with configuration options |
| `AddText(string)`                            | Add simple text                     |
| `AddSeparator(char)`                         | Add a separator line                |
| `FeedLine(int)`                              | Insert blank lines                  |
| `AddTable(Action<Table>)`                    | Create and configure a table        |
| `AddImage(string, Action<ImageOptions>)`     | Add image from file path            |
| `AddBarcode(string, Action<BarcodeOptions>)` | Add barcode with configuration      |
| `ExecuteAsync(CancellationToken)`            | Execute all queued operations       |

#### Text Configuration

```csharp
.AddText("Your text", x => x
    .Alignment(HorizontalAlignment.Center)
    .TextSize(TextSize.DoubleHeight)
    .TextWrap()
    .VerticalAlignment(VerticalAlignment.Center))
```

#### Table Configuration

```csharp
.AddTable(t => t
    .AddSeparator()
    .AddRow(r => r
        .AddRowItem("Header", i => i
            .HorizontalAlignment(HorizontalAlignment.Left)
            .VerticalAlignment(VerticalAlignment.Center)
            .TextWrap())
        .AddRowItem("Value", 12, i => i
            .HorizontalAlignment(HorizontalAlignment.Right)))
    .AddSeparator()
    .AddRow(r => r.AddRowItem("Data row"))
    .FeedLine())
```

### PrinterOptions

Configuration class for printer settings.

```csharp
public class PrinterOptions
{
    public int MaxLineCharacter { get; set; }    // Characters per line
    public string PortName { get; set; }         // COM port (e.g., "COM1")
    public int BaudRate { get; set; } = 9600;    // Communication speed
    public bool OpenDrawer { get; set; }         // Open cash drawer after printing
    public bool CutPaper { get; set; }           // Cut paper after printing
}
```

## 🛠️ Advanced Usage

### Complex Table with Multiple Columns

```csharp
await context
    .AddTable(t => t
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("Product", i => i
                .HorizontalAlignment(HorizontalAlignment.Left)
                .TextWrap())
            .AddRowItem("Qty", 8, i => i
                .HorizontalAlignment(HorizontalAlignment.Center))
            .AddRowItem("Unit Price", 12, i => i
                .HorizontalAlignment(HorizontalAlignment.Right))
            .AddRowItem("Total", 12, i => i
                .HorizontalAlignment(HorizontalAlignment.Right)))
        .AddSeparator()
        .AddRow(r => r.AddRowItem("Premium Coffee Beans"))
        .AddRow(r => r
            .AddRowItem(" 2", 4)
            .AddRowItem(" x ", 3)
            .AddRowItem("$12.99", 12, i => i.HorizontalAlignment(HorizontalAlignment.Right))
            .AddRowItem("$25.98", 12, i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .AddSeparator()
        .AddRow(r => r
            .AddRowItem("Subtotal", 8)
            .AddRowItem("$25.98", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
        .FeedLine()
        .AddSeparator()
        .FeedLine())
    .ExecuteAsync();
```

### Text with Advanced Formatting

```csharp
await context
    .AddText("STORE NAME", x => x
        .Alignment(HorizontalAlignment.Center)
        .TextSize(TextSize.DoubleHeight))
    .FeedLine()
    .AddText("This is a very long line that will be wrapped automatically based on the MaxLineCharacter setting",
             x => x.TextWrap())
    .AddSeparator()
    .ExecuteAsync();
```

### Image and Barcode Printing

```csharp
await context
    .AddImage(@".\Assets\logo.jpg", x => x.Filename("Company Logo"))
    .FeedLine()
    .AddBarcode("123456789012", x => x
        .Height(75)
        .Alignment(HorizontalAlignment.Center))
    .FeedLine(2)
    .ExecuteAsync();
```

### Async Operations with Cancellation

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

try
{
    await context
        .AddText("Processing...", x => x.Alignment(HorizontalAlignment.Center))
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
