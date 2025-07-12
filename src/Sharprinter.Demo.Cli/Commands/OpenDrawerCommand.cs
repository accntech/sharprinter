using Sharprinter.Demo.Cli.Settings;
using Spectre.Console.Cli;

namespace Sharprinter.Demo.Cli.Commands;

public class OpenDrawerCommand : AsyncCommand<PrintSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, PrintSettings settings)
    {
        var options = new PrinterOptions
        {
            PortName = settings.Port,
            BaudRate = settings.BaudRate
        };
        var ctx = new PrinterContext(options);

        await ctx.OpenDrawerAsync();
        return 0;
    }
}