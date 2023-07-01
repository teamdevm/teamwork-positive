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

    public Task<object> ShowAsync (Window parent)
    {
        if (ViewModel!.Buttons == MessageBoxButtons.Ok || 
            ViewModel!.Buttons == MessageBoxButtons.OkCancel || 
            ViewModel!.Buttons == MessageBoxButtons.TextField ||
            ViewModel!.Buttons == MessageBoxButtons.NumField)
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
            ViewModel!.Buttons == MessageBoxButtons.TextField ||
            ViewModel!.Buttons == MessageBoxButtons.NumField)
        {
            AddButton("Cancel");
        }

        TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        Closed += (send, args) => {
            if (ViewModel!.Buttons == MessageBoxButtons.TextField)
            {
                if (ViewModel!.Result == "Cancel")
                    tcs.TrySetResult(string.Empty);
                else
                    tcs.TrySetResult(ViewModel!.Field);
            }
            else if (ViewModel!.Buttons == MessageBoxButtons.NumField)
            {
                if (ViewModel!.Result == "Cancel")
                    tcs.TrySetResult(0);
                else
                    tcs.TrySetResult(ViewModel!.DResult);
            }
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

        if (caption == "Ok" || caption == "Yes")
        {
            btn.IsDefault = true;
        }
        else
        if (caption == "Cancel" || caption == "No")
        {
            btn.IsCancel = true;
        }

        buttonPanel.Children.Add(btn);
    }
}