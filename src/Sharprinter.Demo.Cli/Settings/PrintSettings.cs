using System.ComponentModel;
using Spectre.Console.Cli;

namespace Sharprinter.Demo.Cli.Settings;

public class PrintSettings : CommandSettings
{
    [CommandOption("-p|--port <PORT>")]
    [Description("Serial port to connect to, e.g., COM1")]
    public string Port { get; set; } = string.Empty;

    [CommandOption("-r|--rate <BAUDRATE>")]
    [Description("Baud rate for the serial connection")]
    public int BaudRate { get; set; } = 9600;

    [CommandOption("-m|--max-character <MAXLINE>")]
    [Description("Maximum number of characters per line")]
    public int? MaxLineCharacter { get; set; } = null;

    [CommandOption("-o|--open-drawer <OPEN-DRAWER>")]
    [Description("Open the cash drawer after printing")]
    [DefaultValue(false)]
    public bool? OpenDrawer { get; set; }

    [CommandOption("-c|--cut-paper <CUT-PAPER>")]
    [Description("Cut the paper after printing after printing (only for printers that support this)")]
    [DefaultValue(false)]
    public bool? CutPaper { get; set; }
}