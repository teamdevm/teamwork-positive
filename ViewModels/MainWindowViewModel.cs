using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.ComponentModel;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Controls;
using Avalonia.Themes.Fluent;
using System.Threading.Tasks;
using Documently.Models;
namespace Documently.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase curWindow;
    private ViewModelBase prevWindow;
    private string curTitle;
    private string prevTitle;
    private bool mode;
    private string workingPath;
    private CollectionViewModel collectionWindow;
    private EditViewModel editWindow;
    private FillViewModel fillWindow;

    public Interaction<FileDialogFilter, string> OpenDialogInteraction { get; }
    public Interaction<FileDialogFilter, string> SaveDialogInteraction { get; }
    public Interaction<MessageBoxViewModel, string> ConfirmDialogInteraction { get; }
    public Interaction<MessageBoxViewModel, string> GetAnswerInteraction { get; }
    public ReactiveCommand<Unit, Unit> ActionFillTemplate { get; }
    public ReactiveCommand<Unit, Unit> ActionUploadTemplate { get; }
    public ReactiveCommand<Unit, Unit> ActionRemoveTemplate { get; }
    public ReactiveCommand<Unit, Unit> ActionNewCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionNewSubCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionRenameCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionRemoveCategory { get; }

    public ReactiveCommand<Unit, Unit> ActionDocSave { get; }
    public ReactiveCommand<Unit, Unit> ActionDocSaveAs { get; }
    //public ReactiveCommand<Unit, Unit> ActionGetCount { get; }
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
    
    public MainWindowViewModel() 
    { 
        collectionWindow = new CollectionViewModel();
        editWindow = new EditViewModel();
        fillWindow = new FillViewModel();
        CurWindow = collectionWindow;
        CurTitle = "Коллекция шаблонов";
        mode = false;
        workingPath = String.Empty;
        
        Application.Current.Resources["Theme"] = Application.Current.Resources["Dark"];
        Application.Current.Resources["PanelBackground"] = Application.Current.Resources["LightBrush"];
        Application.Current.Resources["FirstBlockBackground"] = Application.Current.Resources["AlmostWhiteBrush"];
        Application.Current.Resources["SecondBlockBackground"] = Application.Current.Resources["LightBlueBrush"];
        Application.Current.Resources["ButtonColor"] = Application.Current.Resources["DarkBlueBrush"];

        OpenDialogInteraction = new Interaction<FileDialogFilter, string>();
        SaveDialogInteraction = new Interaction<FileDialogFilter, string>();
        //EditDialogInteraction = new Interaction<EditWindowViewModel, Student>();
        ConfirmDialogInteraction = new Interaction<MessageBoxViewModel, string>();
        GetAnswerInteraction = new Interaction<MessageBoxViewModel, string>();
        //CountTemplatesInteraction = new Interaction<MessageBoxViewModel, int>();

        //ActionFileNew = ReactiveCommand.CreateFromTask(FileNew);
        ActionFillTemplate = ReactiveCommand.CreateFromTask(FillTemplate);
        ActionUploadTemplate = ReactiveCommand.CreateFromTask(UploadTemplate);
        ActionRemoveTemplate = ReactiveCommand.CreateFromTask(RemoveTemplate);
        ActionNewCategory = ReactiveCommand.CreateFromTask(AddCategory);
        ActionNewSubCategory = ReactiveCommand.CreateFromTask(AddSubCategory);
        ActionRenameCategory = ReactiveCommand.CreateFromTask(RenameCategory);
        ActionRemoveCategory = ReactiveCommand.CreateFromTask(RemoveCategory);
        ActionDocSave = ReactiveCommand.CreateFromTask(DocSave);
        ActionDocSaveAs = ReactiveCommand.CreateFromTask(DocSaveAs);
        //ActionGetCount = ReactiveCommand.CreateFromTask(GetCount);

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
        collectionWindow.db.Close();
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

    //public async Task GetCount()
    //{
    //    MessageBoxViewModel msg;
    //    if (collectionWindow.SelectedTemplate is null)
    //    {
    //        msg = new MessageBoxViewModel("Шаблон не выбран.", MessageBoxButtons.Ok);
    //        await ConfirmDialogInteraction.Handle(msg);
    //        return;
    //    }
    //    msg = new MessageBoxViewModel("Введите количество экземпляров:", MessageBoxButtons.TextField);
    //    string result = await GetAnswerInteraction.Handle(msg);
    //    if (string.IsNullOrEmpty(result)) return;
    //    if (!int.TryParse(result, out fillWindow.count))
    //    {
    //        msg = new MessageBoxViewModel("Введите число.", MessageBoxButtons.Ok);
    //        await ConfirmDialogInteraction.Handle(msg);
    //    }
    //}
    
    public async Task SwitchToFill()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedTemplate is null)
        {
            msg = new MessageBoxViewModel("Шаблон не выбран.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        msg = new MessageBoxViewModel("Введите количество экземпляров:", MessageBoxButtons.TextField);
        string result = await GetAnswerInteraction.Handle(msg);
        if (string.IsNullOrEmpty(result)) return;

        if (!int.TryParse(result, out fillWindow.count))
        {
            msg = new MessageBoxViewModel("Введите число.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
        }
        fillWindow = new FillViewModel(new Backend(), 
            collectionWindow.db.FetchTemplate(collectionWindow.SelectedTemplate),
            fillWindow.count);

        CurWindow = fillWindow;
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
                Mode = !Mode;
                if (Mode)
                {
                    f.Mode = FluentThemeMode.Dark;
                    var res = Application.Current.Resources;
                    res["Theme"] = res["Light"];
                    res["PanelBackground"] = res["DarkBrush"];
                    res["FirstBlockBackground"] = res["DarkGrayBrush"];
                    res["SecondBlockBackground"] = res["DarkPurpleBrush"];
                    res["ButtonColor"] = res["LightPurpleBrush"];
                }
                else
                {
                    f.Mode = FluentThemeMode.Light;
                    var res = Application.Current.Resources;
                    res["Theme"] = res["Dark"];
                    res["PanelBackground"] = res["LightBrush"];
                    res["FirstBlockBackground"] = res["AlmostWhiteBrush"];
                    res["SecondBlockBackground"] = res["LightBlueBrush"];
                    res["ButtonColor"] = res["DarkBlueBrush"];
                }
                break;
            }
        }
    }

    private async Task UploadTemplate()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedCategory is null)
        {
            msg = new MessageBoxViewModel("Категория не выбрана.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        FileDialogFilter Filter = new FileDialogFilter()
        {
            Name = "Текстовые файлы",
            Extensions = { "doc", "docx", "dot", "dotx", "odt", "ott", "rtf", "txt" }
        };
        string result = await OpenDialogInteraction.Handle(Filter);
        if (string.IsNullOrEmpty(result)) return;
        try
        {
            collectionWindow.UploadTemplate(result);
        }
        catch (Exception ex)
        {
            msg = new MessageBoxViewModel(ex.Message, MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
        }
    }
    private async Task FillTemplate()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedTemplate is null)
        {
            msg = new MessageBoxViewModel(
                "Шаблон не выбран.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        else
            await SwitchToFill();
    }
    private async Task RemoveTemplate()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedTemplate is null)
        {
            msg = new MessageBoxViewModel("Шаблон не выбран.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        msg = new MessageBoxViewModel("Удалить шаблон?", MessageBoxButtons.YesNo);
        string result = await ConfirmDialogInteraction.Handle(msg);
        if (string.IsNullOrEmpty(result)) return;
        if (result == "Yes")
            collectionWindow.RemoveTemplate();
    }
    private async Task DocSave()
    {
        if (string.IsNullOrEmpty(workingPath))
            await DocSaveAs();
    }
    private async Task DocSaveAs()
    {
        FileDialogFilter Filter = new FileDialogFilter()
        {
            Name = "Текстовые файлы",
            Extensions = { "docx" }
        };
        string result = await SaveDialogInteraction.Handle(Filter);
        if (string.IsNullOrEmpty(result)) return;
        workingPath = result;
        fillWindow.result = result;
        fillWindow.GetTemplate();
    }
    private async Task AddCategory()
    {
        MessageBoxViewModel msg = new MessageBoxViewModel(
                "Введите название категории:", MessageBoxButtons.TextField);
        string result = await GetAnswerInteraction.Handle(msg);
        if (string.IsNullOrEmpty(result)) return;
        //проверка существования категории
        collectionWindow.AddCategory(result);
    }
    private async Task AddSubCategory()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedCategory is null)
        {
            msg = new MessageBoxViewModel("Категория не выбрана.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        msg = new MessageBoxViewModel(
                "Введите название подкатегории:", MessageBoxButtons.TextField);
        string result = await GetAnswerInteraction.Handle(msg);
        if (string.IsNullOrEmpty(result)) return;
        //проверка существования категории
        collectionWindow.AddSubCategory(result);
    }
    private async Task RenameCategory()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedCategory is null)
        {
            msg = new MessageBoxViewModel("Категория не выбрана.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        msg = new MessageBoxViewModel(
                "Введите название категории:", MessageBoxButtons.TextField);
        string result = await GetAnswerInteraction.Handle(msg);
        if (string.IsNullOrEmpty(result)) return;
        //проверка существования категории
        collectionWindow.RenameCategory(result);
    }
    private async Task RemoveCategory()
    {
        MessageBoxViewModel msg;
        if (collectionWindow.SelectedCategory is null)
        {
            msg = new MessageBoxViewModel("Категория не выбрана.", MessageBoxButtons.Ok);
            await ConfirmDialogInteraction.Handle(msg);
            return;
        }
        msg = new MessageBoxViewModel("Удалить категорию?", MessageBoxButtons.YesNo);
        string result = await ConfirmDialogInteraction.Handle(msg);
        if (string.IsNullOrEmpty(result)) return;
        if (result == "Yes")
            collectionWindow.RemoveCategory();
    }

    

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
