using System.Collections.ObjectModel;
using System.Collections.Generic;
using ReactiveUI;
using System.IO;
using Documently.Models;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia.Controls;
using System.Threading.Tasks;
using Documently.ViewModels;

namespace Documently.ViewModels;

public class CollectionViewModel : ViewModelBase
{
    public TemplateDatabase db;
    Category selectedCategory;
    List<Template> templates;
    Template selectedTemplate;
    //MainWindowViewModel mw;

    

    public ObservableCollection<Category> Categories => db.Categories;

    public Category SelectedCategory 
    { 
        get => selectedCategory; 
        set { Templates = db.GetTemplates(value); selectedCategory = value; } 
    }
    public List<Template> Templates 
    {
        get => templates;
        set => this.RaiseAndSetIfChanged(ref templates, value);
    }
    public Template SelectedTemplate
    {
        get => selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref selectedTemplate, value);
    }
    public CollectionViewModel() 
    {
        db = new TemplateDatabase("Documents.db");
        templates = new List<Template>();
        selectedCategory = null!;
        selectedTemplate = null!;
        //ActionUploadTemplate = ReactiveCommand.CreateFromTask(UploadTemplate);

        //mw = new MainWindowViewModel();
    }
    ~CollectionViewModel()
    {
        db.Close();
    }
    public void AddCategory(string name)
    {
        //добавить диалоговое окно
        
        db.AddCategory(name);
    }
    public void AddSubCategory(string name)
    {
        //добавить диалоговое окно
        db.AddSubCategory(SelectedCategory, name);
    }
    public void UploadTemplate(string path)
    {
        //добавить диалоговое окно
        db.AddTemplate(path, SelectedCategory);
        Templates = db.GetTemplates(SelectedCategory);
    }
    //public void FillSelected ()
    //{
    //    FillViewModel fillViewModel = new FillViewModel(new Backend(), SelectedTemplate);
    //}
}
