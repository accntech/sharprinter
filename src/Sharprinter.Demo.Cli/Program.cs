using Sharprinter.Demo.Cli.Commands;
using Sharprinter.Demo.Cli.Settings;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<OpenDrawerCommand>("open-drawer")
        .WithDescription("Open the cash drawer")
        .WithExample("open-drawer", "COM3", "-r", "9600");
    config.AddBranch<PrintSettings>("print", p =>
    {
        p.AddCommand<PrintReceiptCommand>("receipt")
            .WithDescription("Print a sample receipt")
            .WithExample("print receipt", "COM3", "-r", "9600");
    });
});

return await app.RunAsync(args);