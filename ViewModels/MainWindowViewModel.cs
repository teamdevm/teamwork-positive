using ReactiveUI;
namespace Documently.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase curWindow;
    private ViewModelBase prevWindow;
    public ViewModelBase CurWindow 
    { 
        get => curWindow; 
        set => this.RaiseAndSetIfChanged(ref curWindow, value); 
    }
    public ViewModelBase PrevWindow
    {
        get => prevWindow;
        set => this.RaiseAndSetIfChanged(ref prevWindow, value);
    }

    private CollectionViewModel collectionWindow;
    private CreateViewModel createWindow;
    private EditViewModel editWindow;
    private FillViewModel fillWindow;

    public MainWindowViewModel() 
    { 
        collectionWindow = new CollectionViewModel();
        createWindow = new CreateViewModel();
        editWindow = new EditViewModel();
        fillWindow = new FillViewModel();
        CurWindow = collectionWindow;
    }
    public void SwitchToCollect()
    {
        CurWindow = collectionWindow;
    }
    public void SwitchToPrevious()
    {
        CurWindow = PrevWindow;
    }
    public void SwitchToCreate()
    {
        CurWindow = createWindow;
        PrevWindow = collectionWindow;
    }
    public void SwitchToEdit()
    {
        CurWindow = editWindow;
        PrevWindow = collectionWindow;
    }
    public void SwitchToFill()
    {
        CurWindow = fillWindow;
        PrevWindow = collectionWindow;
    }

    public string Greeting => "Welcome to Avalonia!";
}
