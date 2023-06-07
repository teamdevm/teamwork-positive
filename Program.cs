using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Documently;

class Program
{
    // // Initialization code. Don't use any Avalonia, third-party APIs or any
    // // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // // yet and stuff might break.
    // [STAThread]
    // public static void Main(string[] args) => BuildAvaloniaApp()
    //     .StartWithClassicDesktopLifetime(args);

    // // Avalonia configuration, don't remove; also used by visual designer.
    // public static AppBuilder BuildAvaloniaApp()
    //     => AppBuilder.Configure<App>()
    //         .UsePlatformDetect()
    //         .LogToTrace()
    //         .UseReactiveUI();

    public static void Main (string[] args)
    {
        WordprocessingDocument wdoc = WordprocessingDocument.Open("Assets/prikaz-o-vstuplenii-v-dolzhnost-rukovoditelya.docx", false);
        StreamWriter wrt = new StreamWriter("Assets/prikaz-o-vstuplenii-v-dolzhnost-rukovoditelya.xml");
        string xml = wdoc.ToFlatOpcString();
        wrt.Write(xml);
        wrt.Close();
        wdoc.Close();
    }
}
