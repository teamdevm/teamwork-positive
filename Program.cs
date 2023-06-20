using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Collections.ObjectModel;
using Documently.Models;

namespace Documently;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();

    // public static void Main (string[] args)
    // {
    //     Backend b = new Backend();
    //     b.Setup("/home/eve/Documents/teamwork-positive/Documents/Информационные документы/Шаблоны информационных документов/Объяснительная.docx", "/tmp", "Test");
    //     ObservableCollection<Field> c = b.GetFields();
    //     foreach (Field f in c)
    //     {
    //         f.Value = "TEST";
    //     }
    //     b.Fill(c);
    // }
}
