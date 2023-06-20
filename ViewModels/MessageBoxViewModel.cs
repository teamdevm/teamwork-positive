using System.Reactive;
using ReactiveUI;

namespace Documently.ViewModels;

public enum MessageBoxButtons
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel
}

public enum MessageBoxResult
{
    Ok,
    Cancel,
    Yes,
    No
}

public class MessageBoxViewModel : ReactiveObject
{
    private string message;
    private MessageBoxButtons buttons;
    private MessageBoxResult result;

    public MessageBoxViewModel (string msg, MessageBoxButtons btn)
    {
        message = msg;
        buttons = btn;
        result = MessageBoxResult.Cancel;
    }
    public string Message
    {
        get => message;
        set => this.RaiseAndSetIfChanged(ref message, value);
    }
    public MessageBoxButtons Buttons
    {
        get => buttons;
        set => this.RaiseAndSetIfChanged(ref buttons, value);
    }
    public MessageBoxResult Result
    {
        get => result;
        set => this.RaiseAndSetIfChanged(ref result, value);
    }
}