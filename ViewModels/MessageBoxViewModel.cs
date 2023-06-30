using System.Reactive;
using ReactiveUI;

namespace Documently.ViewModels;

public enum MessageBoxButtons
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel,
    TextField,
    NumField
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
    private object field;
    private bool is_visible_text; 
    private bool is_visible_num;
    private MessageBoxButtons buttons;
    private object result;
    private double d_result;

    public MessageBoxViewModel(string msg, MessageBoxButtons btn)
    {
        message = msg;
        field = null;
        if (btn == MessageBoxButtons.TextField)
        {
            is_visible_text = true;
            is_visible_num = false;
        }
        else if (btn == MessageBoxButtons.NumField)
        {
            is_visible_text = false;
            is_visible_num = true;
        }
        else
        {
            is_visible_text = false;
            is_visible_num = false;
        }
        buttons = btn;
        result = "Cancel";
        d_result = 1;
    }
    public string Message
    {
        get => message;
        set => this.RaiseAndSetIfChanged(ref message, value);
    }
    public object Field
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool IsVisibleText
    {
        get => is_visible_text;
        set => this.RaiseAndSetIfChanged(ref is_visible_text, value);
    }
    public bool IsVisibleNum
    {
        get => is_visible_num;
        set => this.RaiseAndSetIfChanged(ref is_visible_num, value);
    }
    public MessageBoxButtons Buttons
    {
        get => buttons;
        set => this.RaiseAndSetIfChanged(ref buttons, value);
    }
    public object Result
    {
        get => result;
        set => this.RaiseAndSetIfChanged(ref result, value);
    }
    public double DResult
    {
        get => d_result;
        set => this.RaiseAndSetIfChanged(ref d_result, value);
    }
}