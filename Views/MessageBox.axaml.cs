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

    public Task<string> ShowAsync (Window parent)
    {
        if (ViewModel!.Buttons == MessageBoxButtons.Ok || 
            ViewModel!.Buttons == MessageBoxButtons.OkCancel || 
            ViewModel!.Buttons == MessageBoxButtons.TextField)
        {
            AddButton("Ok");
        }
        if (ViewModel!.Buttons == MessageBoxButtons.YesNo || 
            ViewModel!.Buttons == MessageBoxButtons.YesNoCancel)
        {
            AddButton("Yes");
            AddButton("No");
        }
        if (ViewModel!.Buttons == MessageBoxButtons.OkCancel || 
            ViewModel!.Buttons == MessageBoxButtons.YesNoCancel ||
            ViewModel!.Buttons == MessageBoxButtons.TextField)
        {
            AddButton("Cancel");
        }


        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

        Closed += (send, args) => {
            if (ViewModel!.Buttons == MessageBoxButtons.TextField)
                tcs.TrySetResult(ViewModel!.Field);
            else
                tcs.TrySetResult(ViewModel!.Result);
        };

        ShowDialog(parent);

        return tcs.Task;
    }

    private void AddButton (string caption)
    {
        StackPanel buttonPanel = this.FindControl<StackPanel>("Buttons");
        Button btn = new Button();

        btn.Content = caption;
        btn.Click += (send, args) => {
            ViewModel!.Result = caption;
            this.Close();
        };

        buttonPanel.Children.Add(btn);
    }
}