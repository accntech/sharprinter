using System.Diagnostics;
using System.Text;
using Sharprinter.Demo.Cli.Settings;
using Spectre.Console.Cli;

namespace Sharprinter.Demo.Cli.Commands;

public class PrintFileCommand : AsyncCommand<PrintSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, PrintSettings settings)
    {
        var outputFilePath = settings.OutputFile ?? $"receipt_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

        var options = new PrinterOptions
        {
            PortName = settings.Port,
            BaudRate = settings.BaudRate,
            MaxLineCharacter = settings.MaxLineCharacter ?? 32,
            OpenDrawer = settings.OpenDrawer ?? false,
            CutPaper = settings.CutPaper ?? false
        };

        await using var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8);
        writer.AutoFlush = true;

        var printer = new PrinterContext(new FilePrinter(writer, options.MaxLineCharacter), options);
        await printer.PrintAsync();

        Process.Start(new ProcessStartInfo
        {
            FileName = outputFilePath,
            UseShellExecute = true
        });

        return 0;
    }
}