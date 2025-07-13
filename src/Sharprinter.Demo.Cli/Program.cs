using Sharprinter.Demo.Cli.Commands;
using Sharprinter.Demo.Cli.Settings;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<OpenDrawerCommand>("open-drawer")
        .WithDescription("Open the cash drawer")
        .WithExample("open-drawer", "-p COM1", "-r 9600");
    config.AddBranch<PrintSettings>("print", p =>
    {
        p.AddCommand<PrintReceiptCommand>("receipt")
            .WithDescription("Print a sample receipt directly to the printer")
            .WithExample("print", "receipt", "-p COM1", "-r", "-r 9600");
        
        p.AddCommand<PrintConsoleCommand>("console")
            .WithDescription("Print a sample receipt to the console")
            .WithExample("print", "console");
    });
});

return await app.RunAsync(args);