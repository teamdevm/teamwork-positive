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

    }
    private async Task ShowMessage(InteractionContext<MessageBoxViewModel, MessageBoxResult> i)
    {
        MessageBox dialog = new MessageBox();
        dialog.DataContext = i.Input;
        MessageBoxResult result = await dialog.ShowAsync(this);
        i.SetOutput(result);
    }
}