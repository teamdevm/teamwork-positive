using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Documently.ViewModels;

namespace Documently.Views;

public partial class MessageBox : ReactiveWindow<MessageBoxViewModel>
{
    public MessageBox()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public Task<MessageBoxResult> ShowAsync (Window parent)
    {
        if (ViewModel!.Buttons == MessageBoxButtons.Ok || ViewModel!.Buttons == MessageBoxButtons.OkCancel)
        {
            AddButton("Ok", MessageBoxResult.Ok);
        }
        if (ViewModel!.Buttons == MessageBoxButtons.YesNo || ViewModel!.Buttons == MessageBoxButtons.YesNoCancel)
        {
            AddButton("Yes", MessageBoxResult.Yes);
            AddButton("No", MessageBoxResult.No);
        }
        if (ViewModel!.Buttons == MessageBoxButtons.OkCancel || ViewModel!.Buttons == MessageBoxButtons.YesNoCancel)
        {
            AddButton("Cancel", MessageBoxResult.Cancel);
        }

        TaskCompletionSource<MessageBoxResult> tcs = new TaskCompletionSource<MessageBoxResult>();

        Closed += (send, args) => {
            tcs.TrySetResult(ViewModel!.Result);
        };

        ShowDialog(parent);

        return tcs.Task;
    }

    private void AddButton (string caption, MessageBoxResult r)
    {
        StackPanel buttonPanel = this.FindControl<StackPanel>("Buttons");
        Button btn = new Button();

        btn.Content = caption;
        btn.Click += (send, args) => {
            ViewModel!.Result = r;
            this.Close();
        };

        buttonPanel.Children.Add(btn);
    }
}