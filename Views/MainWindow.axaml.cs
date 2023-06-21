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
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(a => a(((MainWindowViewModel)DataContext).ConfirmDialogInteraction.RegisterHandler(ShowMessage)));
        this.WhenActivated(a => a(((MainWindowViewModel)DataContext).OpenDialogInteraction.RegisterHandler(ShowOpenFileWindow)));
        this.WhenActivated(a => a(((MainWindowViewModel)DataContext).GetNameInteraction.RegisterHandler(ShowMessage)));

    }
    private async Task ShowMessage(InteractionContext<MessageBoxViewModel, string> i)
    {
        MessageBox dialog = new MessageBox();
        dialog.DataContext = i.Input;
        string result = await dialog.ShowAsync(this);
        i.SetOutput(result);
    }
    private async Task ShowOpenFileWindow(InteractionContext<FileDialogFilter, string> i)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters = new List<FileDialogFilter>() { i.Input };
        string[]? result = await dialog.ShowAsync(this);
        if (result is null)
        {
            i.SetOutput(string.Empty);
        }
        else
        {
            i.SetOutput(result[0]);
        }
    }
}