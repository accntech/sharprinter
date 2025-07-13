namespace Sharprinter.Demo.Cli.Commands;

public static class PrinterHelper
{
    public static Task PrintAsync(this PrinterContext context)
    {
        return context
            .Image(@".\Assets\image.jpg", "7-ELEVEN Logo")
            .TextLine("123 Main St. Springfield, USA", false, Alignment.Center)
            .TextLine("VATREG 123456789000", false, Alignment.Center)
            .FeedLine()
            .TextLine("INVOICE", false, Alignment.Center, 1)
            .FeedLine()
            .Table()
            .AddHeader([
                new HeaderItem("Item", 20),
                new HeaderItem("Amount", 5, Alignment.Center)
            ])
            .Create()
            .TextLine("Thank you for your purchase!", true)
            .BarCode("123456789012", Alignment.Center)
            .FeedLine(3)
            .ExecuteAsync();
    }
}