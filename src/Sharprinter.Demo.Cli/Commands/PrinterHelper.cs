namespace Sharprinter.Demo.Cli.Commands;

public static class PrinterHelper
{
    public static Task PrintAsync(this PrinterContext context)
    {
        return context
            .AddImage(@".\Assets\image.jpg", x => x.Filename("7-ELEVEN Logo"))
            .AddText("123 Main St. Springfield, USA", x => x.Alignment(HorizontalAlignment.Center))
            .AddText("VATREG 123456789000", x => x.Alignment(HorizontalAlignment.Center))
            .FeedLine()
            .AddText("INVOICE", x => x.Alignment(HorizontalAlignment.Center).TextSize(TextSize.DoubleHeight))
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
                .AddRow(r => r.AddRowItem("Bugles Crispy Corn Snacks"))
                .AddRow(r => r
                    .AddRowItem(" 1", 4)
                    .AddRowItem(" x ", 3)
                    .AddRowItem("199.50", 10)
                    .AddRowItem("199.50", i => i.HorizontalAlignment(HorizontalAlignment.Right).TextWrap()))
                .AddRow(r => r.AddRowItem("Chex Mix Traditional"))
                .AddRow(r => r
                    .AddRowItem(" 1", 4)
                    .AddRowItem(" x ", 3)
                    .AddRowItem("75.00", 10)
                    .AddRowItem("75.00", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
                .AddSeparator()
                .AddRow(r => r
                    .AddRowItem("Subtotal", 8)
                    .AddRowItem("274.50", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
                .FeedLine()
                .AddRow(r => r
                    .AddRowItem("Amount Due", 10)
                    .AddRowItem("274.50", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
                .AddRow(r => r
                    .AddRowItem("Tender", 6)
                    .AddRowItem("300.00", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
                .AddRow(r => r
                    .AddRowItem("Change", 6)
                    .AddRowItem("25.50", i => i.HorizontalAlignment(HorizontalAlignment.Right)))
                .FeedLine()
                .AddSeparator()
                .FeedLine())
            .AddText("Thank you for your purchase!", x => x.Alignment(HorizontalAlignment.Center))
            .FeedLine()
            .AddBarcode("123456789012", x => x.Height(75).Alignment(HorizontalAlignment.Center))
            .FeedLine(2)
            .ExecuteAsync();
    }
}