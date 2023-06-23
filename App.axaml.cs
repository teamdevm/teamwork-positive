using Aspose.Words;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Documently.ViewModels;
using Documently.Views;

namespace Documently;

public partial class App : Application
{

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        License l = new License();
        l.SetLicense("Aspose.Wordsfor.NET.lic");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(new MainWindowViewModel());
        }

        base.OnFrameworkInitializationCompleted();
    }
}