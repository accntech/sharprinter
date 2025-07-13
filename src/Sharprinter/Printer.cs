using System;
using System.Runtime.InteropServices;

namespace Sharprinter;

/// <summary>
///     Provides a wrapper for the printer SDK functionality.
///     This class handles communication with thermal printers through the native SDK.
/// </summary>
internal class Printer : IPrinter
{
    private IntPtr _intPtr = IntPtr.Zero;

    public void Initialize(string model = "")
    {
        _intPtr = Sdk.InitPrinter(model);
    }

    public void Release()
    {
        Sdk.ReleasePrinter(_intPtr);
    }

    public void OpenPort(string port)
    {
        Sdk.OpenPort(_intPtr, port);
    }

    public void ClosePort()
    {
        Sdk.ClosePort(_intPtr);
    }

    public void CutPaper(int cutMode)
    {
        Sdk.CutPaper(_intPtr, cutMode);
    }

    public void CutPaperWithDistance(int distance)
    {
        Sdk.CutPaperWithDistance(_intPtr, distance);
    }

    public void FeedLine(int lines)
    {
        // use PrintText to ensure proper line spacing with different printers
        Sdk.PrintText(_intPtr, new string('\n', lines), 0, 0);
    }

    public void OpenCashDrawer(int pinMode, int onTime, int ofTime)
    {
        Sdk.OpenCashDrawer(_intPtr, pinMode, onTime, ofTime);
    }

    public void PrintText(string data, int alignment, int textSize)
    {
        Sdk.PrintText(_intPtr, data, alignment, textSize);
    }
    
    public void PrintTextLine(string data, int alignment, int textSize)
    {
        Sdk.PrintText(_intPtr, $"{data}\n", alignment, textSize);
    }

    public void PrintBarCode(int type, string data, int width, int height, int alignment, int position)
    {
        Sdk.PrintBarCode(_intPtr, type, data, width, height, alignment, position);
    }

    public void PrintImage(string filePath, string filename, int scaleMode)
    {
        Sdk.PrintImage(_intPtr, filePath, scaleMode);
    }

    private static class Sdk
    {
        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern IntPtr InitPrinter(string model);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int ReleasePrinter(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int OpenPort(IntPtr intPtr, string port);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int ClosePort(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int WriteData(IntPtr intPtr, byte[] buffer, int size);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int ReadData(IntPtr intPtr, byte[] buffer, int size);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int PrinterInitialize(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int SetTextLineSpace(IntPtr intPtr, int lineSpace);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int CancelPrintDataInPageMode(IntPtr intPtr);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int GetPrinterState(IntPtr intPtr, ref int printerStatus);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int SetCodePage(IntPtr intPtr, int characterSet);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int SetInternationalCharacter(IntPtr intPtr, int characterSet);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int CutPaper(IntPtr intPtr, int cutMode);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int CutPaperWithDistance(IntPtr intPtr, int distance);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int FeedLine(IntPtr intPtr, int lines);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int OpenCashDrawer(IntPtr intPtr, int pinMode, int onTime, int ofTime);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintText(IntPtr intPtr, string data, int alignment, int textSize);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintTextS(IntPtr intPtr, string data);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintBarCode(IntPtr intPtr, int bcType, string bcData, int width, int height,
            int alignment,
            int hriPosition);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintSymbol(IntPtr intPtr, int type, string data, int errLevel, int width, int height,
            int alignment);

        [DllImport("printer.sdk.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int PrintImage(IntPtr intPtr, string filePath, int scaleMode);
    }
}