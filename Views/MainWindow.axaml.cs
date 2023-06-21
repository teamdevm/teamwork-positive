using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Documently.Models;
using Documently.ViewModels;
namespace Documently.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow () => new MainWindow(ViewModel!);
    public MainWindow (MainWindowViewModel m)
    {
        InitializeComponent();

        this.WhenActivated(a => a(m.ConfirmDialogInteraction.RegisterHandler(ShowMessage)));
        this.Closing += m.OnWindowClose;

        DataContext = m;
    }
    private async Task ShowMessage(InteractionContext<MessageBoxViewModel, MessageBoxResult> i)
    {
        MessageBox dialog = new MessageBox();
        dialog.DataContext = i.Input;
        MessageBoxResult result = await dialog.ShowAsync(this);
        i.SetOutput(result);
    }
}