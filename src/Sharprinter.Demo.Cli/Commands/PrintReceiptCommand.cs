using Sharprinter.Demo.Cli.Settings;
using Spectre.Console.Cli;

namespace Sharprinter.Demo.Cli.Commands;

public class PrintReceiptCommand : AsyncCommand<PrintSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, PrintSettings settings)
    {
        var options = new PrinterOptions
        {
            PortName = settings.Port,
            BaudRate = settings.BaudRate,
            MaxLineCharacter = settings.MaxLineCharacter ?? 32,
            OpenDrawer = settings.OpenDrawer ?? false,
            CutPaper = settings.CutPaper ?? false
        };

        var ctx = new PrinterContext(options);

        ctx.TextLine("INVOICE", true, Alignment.Center, 1);
        ctx.FeedLine();

        ctx.TextLine("Sharprinter Demo Receipt", true);
        ctx.FeedLine(2);

        await ctx.ExecuteAsync();

        return 0;
    }
}