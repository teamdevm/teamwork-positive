using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.ComponentModel;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;
using Documently.Models;
using System.Threading.Tasks;

namespace Documently.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase curWindow;
    private ViewModelBase prevWindow;
    private string curTitle;
    private string prevTitle;

    public bool mode;
    //public bool Mode => mode != 0;
    //private string size;

    public Interaction<FileDialogFilter, string> OpenDialogInteraction { get; }
    public Interaction<FileDialogFilter, string> SaveDialogInteraction { get; }
    //public Interaction<EditWindowViewModel, Student> EditDialogInteraction { get; }
    public Interaction<MessageBoxViewModel, MessageBoxResult> ConfirmDialogInteraction { get; }
    //public ReactiveCommand<Unit, Unit> ActionFileNew { get; }
    public ReactiveCommand<Unit, Unit> ActionTemplateFill { get; }
    public ReactiveCommand<Unit, Unit> ActionTemplateUpload { get; }
    //public ReactiveCommand<Unit, Unit> ActionFileSave { get; }
    //public ReactiveCommand<Unit, Unit> ActionFileSaveAs { get; }
    //public ReactiveCommand<Unit, Unit> ActionViewNext { get; }
    //public ReactiveCommand<Unit, Unit> ActionViewPrev { get; }
    //public ReactiveCommand<Unit, Unit> ActionViewFirst { get; }
    //public ReactiveCommand<Unit, Unit> ActionViewLast { get; }
    //public ReactiveCommand<Unit, Unit> ActionNew { get; }
    //public ReactiveCommand<Unit, Unit> ActionEdit { get; }
    //public ReactiveCommand<Unit, Unit> ActionRemove { get; }

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
    public bool Mode
    {
        get => mode;
        set => this.RaiseAndSetIfChanged(ref mode, value);
    }
    
    public string PrevTitle
    {
        get => prevTitle;
        set => this.RaiseAndSetIfChanged(ref prevTitle, value);
    }

    private CollectionViewModel collectionWindow;
    private EditViewModel editWindow;
    private FillViewModel fillWindow;

    public MainWindowViewModel() 
    { 
        collectionWindow = new CollectionViewModel();
        editWindow = new EditViewModel();
        fillWindow = new FillViewModel();
        CurWindow = collectionWindow;
        CurTitle = "Коллекция шаблонов";
        mode = false;
        //Size = "20";
        //ShowDialog = new Interaction<CollectionViewModel, FillViewModel>();

        OpenDialogInteraction = new Interaction<FileDialogFilter, string>();
        SaveDialogInteraction = new Interaction<FileDialogFilter, string>();
        //EditDialogInteraction = new Interaction<EditWindowViewModel, Student>();
        ConfirmDialogInteraction = new Interaction<MessageBoxViewModel, MessageBoxResult>();

        //ActionFileNew = ReactiveCommand.CreateFromTask(FileNew);
        ActionTemplateFill = ReactiveCommand.CreateFromTask(CheckTemplate);
        //ActionTemplateUpload = ReactiveCommand.CreateFromTask(TemplateUpload);

        //ActionFileSave = ReactiveCommand.CreateFromTask(FileSave);
        //ActionFileSaveAs = ReactiveCommand.CreateFromTask(FileSaveAs);
        //ActionViewNext = ReactiveCommand.Create(ViewNext);
        //ActionViewPrev = ReactiveCommand.Create(ViewPrev);
        //ActionViewFirst = ReactiveCommand.Create(ViewFirst);
        //ActionViewLast = ReactiveCommand.Create(ViewLast);
        //ActionNew = ReactiveCommand.CreateFromTask(New);
        //ActionEdit = ReactiveCommand.CreateFromTask(Edit);
        //ActionRemove = ReactiveCommand.Create(Remove);
    }
    public void OnWindowClose (object? sender, CancelEventArgs args)
    {
        // Close the database here
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
        CurWindow = editWindow;
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

    //public bool CheckFileName(ObservableCollection<Field> table)
    //{
    //    string copyName = fileName;

    //    for (int i = 0; i < table.Count; i++)
    //        copyName = copyName.Replace(table[i].Name, "Переменная");

    //    // Path.GetInvalidFileNameChars();

    //    if (copyName.IndexOf("\\") >= 0 || copyName.IndexOf("/") >= 0 || copyName.IndexOf(":") >= 0 ||
    //        copyName.IndexOf("*") >= 0 || copyName.IndexOf("?") >= 0 || copyName.IndexOf("|") >= 0 ||
    //        copyName.IndexOf("<") >= 0 || copyName.IndexOf(">") >= 0 || copyName == "")
    //    {
    //        throw new ArgumentException("Недопустимый символ в имени файла или пустой ввод.Запрещено использовать такие символы, как \\, /, :, *, ?, |, <, > \n" +
    //            "(Знаки <> можно использовать в том случае, если они используются для определения переменной в названии файла)\n" +
    //            "Попробуйте ввести имя еще раз");
    //    }

    //    return true;
    //}
    //private async Task TemplateUpload()
    //{
    //    /* We accept only XML files */
    //    FileDialogFilter xmlFilter = new FileDialogFilter()
    //    {
    //        Extensions = { "xml" }
    //    };

    //    /* Show dialog window and retrieve file path */
    //    string result = await OpenDialogInteraction.Handle(xmlFilter);

    //    /* If no file was selected */
    //    if (string.IsNullOrEmpty(result)) return;

    //    try
    //    {
    //        collectionWindow.db
    //        Content = StudentList.Deserialize(result);
    //        Selection = Content.First();
    //        workingPath = result;
    //        isSaved = true;
    //        UpdateAll();
    //    }
    //    catch
    //    {
    //        MessageBoxViewModel msg = new MessageBoxViewModel("Файл имеет некорректный формат", MessageBoxButtons.Ok);
    //        MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
    //    }
    //}
    private async Task CheckTemplate()
    {
        if (collectionWindow.SelectedTemplate is null)
        {
            MessageBoxViewModel msg = new MessageBoxViewModel(
                "Выберите шаблон", MessageBoxButtons.Ok);
            MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
        }
        else
            SwitchToFill();
        //if (!isSaved)
        //{
        //    MessageBoxViewModel msg = new MessageBoxViewModel("Сохранить изменения?", MessageBoxButtons.YesNoCancel);
        //    MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
        //    switch (res)
        //    {
        //        case MessageBoxResult.Yes: await FileSave(); break;
        //        case MessageBoxResult.Cancel: return;
        //    }
        //}

        /* We accept only XML files */
        //FileDialogFilter xmlFilter = new FileDialogFilter()
        //{
        //    Name = "Списки студентов",
        //    Extensions = { "xml" }
        //};

        ///* Show dialog window and retrieve file path */
        //string result = await OpenDialogInteraction.Handle(xmlFilter);

        ///* If no file was selected */
        //if (string.IsNullOrEmpty(result)) return;

        //try
        //{
        //    Content = StudentList.Deserialize(result);
        //    Selection = Content.First();
        //    workingPath = result;
        //    isSaved = true;
        //    UpdateAll();
        //}
        //catch
        //{
        //    MessageBoxViewModel msg = new MessageBoxViewModel("Файл имеет некорректный формат", MessageBoxButtons.Ok);
        //    MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
        //}
    }

    public void SwitchToFill()
    {
        CurWindow = new FillViewModel(new Backend(), collectionWindow.db.FetchTemplate(collectionWindow.SelectedTemplate));
        PrevWindow = collectionWindow;
        CurTitle = "Заполнить шаблон";
        PrevTitle = "Коллекция шаблонов";
    }

    public void SwitchTheme()
    {
        
        foreach (IStyle s in Application.Current.Styles)
        {
            if (s is FluentTheme f)
            {
                mode = !mode;
                if (mode)
                    f.Mode = FluentThemeMode.Dark;
                else
                    f.Mode = FluentThemeMode.Light;
                //f.Mode = (FluentThemeMode)((int)mode);
            }
        }
    }

    

    //private async Task FileNew()
    //{
    //    if (!isSaved)
    //    {
    //        MessageBoxViewModel msg = new MessageBoxViewModel("Сохранить изменения?", MessageBoxButtons.YesNoCancel);
    //        MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
    //        switch (res)
    //        {
    //            case MessageBoxResult.Yes: await FileSave(); break;
    //            case MessageBoxResult.Cancel: return;
    //        }
    //    }

    //    Content = new StudentList();
    //    Selection = Content.First();
    //    workingPath = string.Empty;
    //    isSaved = true;
    //    UpdateAll();
    //}

    //private async Task FileOpen()
    //{
    //    if (!isSaved)
    //    {
    //        MessageBoxViewModel msg = new MessageBoxViewModel("Сохранить изменения?", MessageBoxButtons.YesNoCancel);
    //        MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
    //        switch (res)
    //        {
    //            case MessageBoxResult.Yes: await FileSave(); break;
    //            case MessageBoxResult.Cancel: return;
    //        }
    //    }

    //    /* We accept only XML files */
    //    FileDialogFilter xmlFilter = new FileDialogFilter()
    //    {
    //        Name = "Списки студентов",
    //        Extensions = { "xml" }
    //    };

    //    /* Show dialog window and retrieve file path */
    //    string result = await OpenDialogInteraction.Handle(xmlFilter);

    //    /* If no file was selected */
    //    if (string.IsNullOrEmpty(result)) return;

    //    try
    //    {
    //        Content = StudentList.Deserialize(result);
    //        Selection = Content.First();
    //        workingPath = result;
    //        isSaved = true;
    //        UpdateAll();
    //    }
    //    catch
    //    {
    //        MessageBoxViewModel msg = new MessageBoxViewModel("Файл имеет некорректный формат", MessageBoxButtons.Ok);
    //        MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
    //    }
    //}

    //private async Task FileSave()
    //{
    //    if (string.IsNullOrEmpty(workingPath))
    //    {
    //        await FileSaveAs();
    //    }
    //    else
    //    {
    //        Content.Serialize(workingPath);
    //        isSaved = true;
    //        UpdateAll();
    //    }
    //}

    //private async Task FileSaveAs()
    //{
    //    /* We accept only XML files */
    //    FileDialogFilter xmlFilter = new FileDialogFilter()
    //    {
    //        Name = "Списки студентов",
    //        Extensions = { "xml" }
    //    };

    //    /* Show dialog window and retrieve file path */
    //    string result = await SaveDialogInteraction.Handle(xmlFilter);

    //    /* If no file was selected */
    //    if (string.IsNullOrEmpty(result)) return;

    //    Content.Serialize(result);
    //    workingPath = result;
    //    isSaved = true;
    //    UpdateAll();
    //}

    //private void ViewNext()
    //{
    //    Selection = Content.Next();
    //    UpdateAll();
    //}

    //private void ViewPrev()
    //{
    //    Selection = Content.Prev();
    //    UpdateAll();
    //}

    //private void ViewFirst()
    //{
    //    Selection = Content.First();
    //    UpdateAll();
    //}

    //private void ViewLast()
    //{
    //    Selection = Content.Last();
    //    UpdateAll();
    //}

    //private async Task New()
    //{
    //    EditWindowViewModel model = new EditWindowViewModel();
    //    Student result = await EditDialogInteraction.Handle(model);
    //    if (result is not null)
    //    {
    //        Content.Add(result);
    //        Selection = Content.This;
    //        isSaved = false;
    //        UpdateAll();
    //    }
    //}

    //private async Task Edit()
    //{
    //    EditWindowViewModel model = new EditWindowViewModel(content.This!);
    //    Student result = await EditDialogInteraction.Handle(model);
    //    if (result is not null)
    //    {
    //        Content.This = result;
    //        Selection = result;
    //        isSaved = false;
    //        UpdateAll();
    //    }
    //}

    //private void Remove()
    //{
    //    Content.Remove();
    //    Selection = Content.This;
    //    isSaved = false;
    //    UpdateAll();
    //}

    //public async void ConfirmOnClose(object? sender, CancelEventArgs args)
    //{
    //    if (!isSaved)
    //    {
    //        args.Cancel = true;
    //        Window win = (Window)sender!;
    //        MessageBoxViewModel msg = new MessageBoxViewModel("Сохранить изменения?", MessageBoxButtons.YesNoCancel);
    //        MessageBoxResult res = await ConfirmDialogInteraction.Handle(msg);
    //        switch (res)
    //        {
    //            case MessageBoxResult.Yes: await FileSave(); win.Close(); break;
    //            case MessageBoxResult.No: isSaved = true; win.Close(); break;
    //        }
    //    }
    //}
}
