using ReactiveUI;
namespace Documently.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase curWindow;
    private ViewModelBase prevWindow;
    private string curTitle;
    private string prevTitle;
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
    public string CurTitle
    {
        get => curTitle;
        set => this.RaiseAndSetIfChanged(ref curTitle, value);
    }
    public string PrevTitle
    {
        get => prevTitle;
        set => this.RaiseAndSetIfChanged(ref prevTitle, value);
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
        CurTitle = "Коллекция шаблонов";
    }
    public void SwitchToCollect()
    {
        CurWindow = collectionWindow;
        CurTitle = "Коллекция шаблонов";
    }
    public void SwitchToPrevious()
    {
        CurWindow = PrevWindow;
        CurTitle = PrevTitle;
    }
    public void SwitchToCreate()
    {
        CurWindow = createWindow;
        CurTitle = "Новый шаблон";
        PrevWindow = collectionWindow;
        PrevTitle = "Коллекция шаблонов";
    }
    public void SwitchToEdit()
    {
        CurWindow = editWindow;
        PrevWindow = collectionWindow;
        CurTitle = "Изменить шаблон";
        PrevTitle = "Коллекция шаблонов";
    }
    public void SwitchToFill()
    {
        CurWindow = fillWindow;
        PrevWindow = collectionWindow;
        CurTitle = "Заполнить шаблон";
        PrevTitle = "Коллекция шаблонов";
    }

    public string Greeting => "Welcome to Avalonia!";
}
