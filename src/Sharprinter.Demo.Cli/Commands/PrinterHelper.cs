namespace Sharprinter.Demo.Cli.Commands;

public static class PrinterHelper
{
    public static Task PrintAsync(this PrinterContext context)
    {
        return context
            .Image(@".\Assets\image.jpg", "7-ELEVEN Logo")
            .TextLine("123 Main St. Springfield, USA", alignment: HorizontalAlignment.Center)
            .TextLine("VATREG 123456789000", alignment: HorizontalAlignment.Center)
            .FeedLine()
            .TextLine("INVOICE", alignment: HorizontalAlignment.Center, textSize: TextSize.DoubleHeight)
            .FeedLine()
            .TextLine("MIN: 1234567890")
            .TextLine("SN: 1A2B3C4D5E6F7G8H9")
            .FeedLine()
            .TextLine("Order No: 123456")
            .TextLine($"Date: {DateTime.Now:MM/dd/yyyy HH:mm:ss}")
            .TextLine("Cashier: John Doe")
            .TextLine("Customer: Jane Smith")
            .Table()
            .AddRowSeparator()
            .AddLine([
                new LineItem("Description", 20, HorizontalAlignment.Left, VerticalAlignment.Center, true),
                new LineItem("Amount", 12, HorizontalAlignment.Center, VerticalAlignment.Center)
            ])
            .AddRowSeparator()
            .AddLine("Bugles Crispy Corn Snacks")
            .AddLine(" 2 x 99.75", "199.50")
            .AddLine("Chex Mix Traditional")
            .AddLine(" 1 x 75", "75.00")
            .AddRowSeparator()
            .AddLine("Subtotal", "274.50")
            .AddEmptyLine()
            .AddLine("Amount Due", "274.50")
            .AddLine("Tender", "300.00")
            .AddLine("CHANGE", "25.50")
            .AddEmptyLine()
            .AddRowSeparator()
            .Create()
            .FeedLine()
            .TextLine("Thank you for your purchase!", alignment: HorizontalAlignment.Center)
            .FeedLine()
            .BarCode("123456789012", 75, alignment: HorizontalAlignment.Center)
            .FeedLine(2)
            .ExecuteAsync();
    }
}