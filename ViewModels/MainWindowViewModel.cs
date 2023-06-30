using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
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
    private HelpViewModel helpWindow;

    public Interaction<List<FileDialogFilter>, string> OpenDialogInteraction { get; }
    public Interaction<List<FileDialogFilter>, string> SaveDialogInteraction { get; }
    public Interaction<MessageBoxViewModel, object> ConfirmDialogInteraction { get; }
    public Interaction<MessageBoxViewModel, object> GetAnswerInteraction { get; }
    public ReactiveCommand<Unit, Unit> ActionFillTemplate { get; }
    public ReactiveCommand<Unit, Unit> ActionUploadTemplate { get; }
    public ReactiveCommand<Unit, Unit> ActionRemoveTemplate { get; }
    public ReactiveCommand<Unit, Unit> ActionNewCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionNewSubCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionRenameCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionRemoveCategory { get; }
    public ReactiveCommand<Unit, Unit> ActionDocSave { get; }
    public ReactiveCommand<Unit, Unit> ActionDocSaveAs { get; }
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
    public bool Mode
    {
        get => mode;
        set => this.RaiseAndSetIfChanged(ref mode, value);
    }
    
    public MainWindowViewModel() 
    { 
        collectionWindow = new CollectionViewModel();
        editWindow = new EditViewModel();
        fillWindow = new FillViewModel();
        helpWindow = new HelpViewModel();

        CurWindow = collectionWindow;
        CurTitle = "Коллекция шаблонов";

        mode = false;
        workingPath = String.Empty;
        
        Application.Current.Resources["Theme"] = Application.Current.Resources["Dark"];
        Application.Current.Resources["PanelBackground"] = Application.Current.Resources["LightBrush"];
        Application.Current.Resources["FirstBlockBackground"] = Application.Current.Resources["AlmostWhiteBrush"];
        Application.Current.Resources["SecondBlockBackground"] = Application.Current.Resources["LightBlueBrush"];
        Application.Current.Resources["ButtonColor"] = Application.Current.Resources["DarkBlueBrush"];

        OpenDialogInteraction = new Interaction<List<FileDialogFilter>, string>();
        SaveDialogInteraction = new Interaction<List<FileDialogFilter>, string>();
        ConfirmDialogInteraction = new Interaction<MessageBoxViewModel, object>();
        GetAnswerInteraction = new Interaction<MessageBoxViewModel, object>();
        
        ActionFillTemplate = ReactiveCommand.CreateFromTask(FillTemplate);
        ActionUploadTemplate = ReactiveCommand.CreateFromTask(UploadTemplate);
        ActionRemoveTemplate = ReactiveCommand.CreateFromTask(RemoveTemplate);
        ActionNewCategory = ReactiveCommand.CreateFromTask(AddCategory);
        ActionNewSubCategory = ReactiveCommand.CreateFromTask(AddSubCategory);
        ActionRenameCategory = ReactiveCommand.CreateFromTask(RenameCategory);
        ActionRemoveCategory = ReactiveCommand.CreateFromTask(RemoveCategory);
        ActionDocSave = ReactiveCommand.CreateFromTask(DocSave);
        ActionDocSaveAs = ReactiveCommand.CreateFromTask(DocSaveAs);
    }
    public void OnWindowClose (object? sender, CancelEventArgs args)
    {
        // Close the database here
        collectionWindow.db.Close();
    }
    public void SwitchToPrevious()
    {
        CurWindow = PrevWindow;
        CurTitle = PrevTitle;
    }
    public void SwitchToCollect()
    {
        CurWindow = collectionWindow;
        workingPath = String.Empty;
        CurTitle = "Коллекция шаблонов";
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
    public void SwitchToHelp()
    {
        PrevWindow = CurWindow;
        PrevTitle = CurTitle;
        CurTitle = "Справка";
        CurWindow = helpWindow;
    }

    public async Task SwitchToFill()
    {
        MessageBoxViewModel msg = new MessageBoxViewModel("Введите количество экземпляров:", MessageBoxButtons.NumField);
        object res = await GetAnswerInteraction.Handle(msg);
        if (res is double result)
        {
            if (result == 0) return;
            try
            {
                fillWindow = new FillViewModel(new Backend(), 
                    collectionWindow.db.FetchTemplate(collectionWindow.SelectedTemplate),
                    (int)result);

                CurWindow = fillWindow;
                PrevWindow = collectionWindow;
                CurTitle = "Заполнить шаблон";
                PrevTitle = "Коллекция шаблонов";
            }
            catch (Exception ex)
            {
                msg = new MessageBoxViewModel(ex.Message, MessageBoxButtons.Ok);
                await ConfirmDialogInteraction.Handle(msg);
            }
        }
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
        List<FileDialogFilter> Filters = new List<FileDialogFilter>();
        Filters.Add(new FileDialogFilter() { Name = "Документ Word 2003", Extensions = { "doc" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ Word 2007", Extensions = { "docx" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ ODT", Extensions = { "odt" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ RTF", Extensions = { "rtf" } });
        Filters.Add(new FileDialogFilter() { Name = "Обычный текст", Extensions = { "txt" } });
        string result = await OpenDialogInteraction.Handle(Filters);
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
        object res = await ConfirmDialogInteraction.Handle(msg);
        if (res is string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            if (result == "Yes")
                collectionWindow.RemoveTemplate();
        }
    }
    private async Task DocSave()
    {
        if (string.IsNullOrEmpty(workingPath))
            await DocSaveAs();
        fillWindow.GetTemplate();
        MessageBoxViewModel msg = new MessageBoxViewModel("Сохранено.", MessageBoxButtons.Ok);
        await ConfirmDialogInteraction.Handle(msg);
    }
    private async Task DocSaveAs()
    {
        List<FileDialogFilter> Filters = new List<FileDialogFilter>();
        Filters.Add(new FileDialogFilter() { Name = "Документ Word 2003", Extensions = { "doc" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ Word 2007", Extensions = { "docx" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ ODT", Extensions = { "odt" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ PDF", Extensions = { "pdf" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ RTF", Extensions = { "rtf" } });
        Filters.Add(new FileDialogFilter() { Name = "Документ HTML", Extensions = { "html" } });
        Filters.Add(new FileDialogFilter() { Name = "Обычный текст", Extensions = { "txt" } });
        string result = await SaveDialogInteraction.Handle(Filters);
        if (string.IsNullOrEmpty(result)) return;
        workingPath = result;
        fillWindow.result = result;
        fillWindow.GetTemplate();
    }
    private async Task AddCategory()
    {
        MessageBoxViewModel msg = new MessageBoxViewModel(
                "Введите название категории:", MessageBoxButtons.TextField);
        object res = await GetAnswerInteraction.Handle(msg);
        if (res is string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            //проверка существования категории
            collectionWindow.AddCategory(result);
        }
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
        object res = await GetAnswerInteraction.Handle(msg);
        if (res is string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            //проверка существования категории
            collectionWindow.AddSubCategory(result);
        }
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
        object res = await GetAnswerInteraction.Handle(msg);
        if (res is string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            //проверка существования категории
            collectionWindow.RenameCategory(result);
        }
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
        object res = await GetAnswerInteraction.Handle(msg);
        if (res is string result)
        {
            if (string.IsNullOrEmpty(result)) return;
            if (result == "Yes")
                collectionWindow.RemoveCategory();
        }
    }

}
