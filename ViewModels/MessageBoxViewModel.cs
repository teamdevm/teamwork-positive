using System.Reactive;
using ReactiveUI;

namespace Documently.ViewModels;

public enum MessageBoxButtons
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel,
    TextField
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
    private string field;
    private bool is_visible;
    private MessageBoxButtons buttons;
    private string result;

    public MessageBoxViewModel(string msg, MessageBoxButtons btn)
    {
        message = msg;
        field = string.Empty;
        if (btn == MessageBoxButtons.TextField)
            is_visible = true;
        else
            is_visible = false;
        buttons = btn;
        result = "Cancel";
    }
    public string Message
    {
        get => message;
        set => this.RaiseAndSetIfChanged(ref message, value);
    }
    public string Field
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool IsVisible
    {
        get => is_visible;
        set => this.RaiseAndSetIfChanged(ref is_visible, value);
    }
    public MessageBoxButtons Buttons
    {
        get => buttons;
        set => this.RaiseAndSetIfChanged(ref buttons, value);
    }
    public string Result
    {
        get => result;
        set => this.RaiseAndSetIfChanged(ref result, value);
    }
}