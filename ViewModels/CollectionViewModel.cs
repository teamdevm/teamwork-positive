using System.Collections.ObjectModel;
using System.Collections.Generic;
using ReactiveUI;
using System.IO;
using Documently.Models;
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
        //mw = new MainWindowViewModel();
    }
    ~CollectionViewModel()
    {
        db.Close();
    }
    public void AddCategory()
    {
        //добавить диалоговое окно
        
        db.AddCategory("новая категория");
    }
    public void AddSubCategory()
    {
        //добавить диалоговое окно
        db.AddSubCategory(SelectedCategory, "новая подкатегория");
    }
    public void UploadTemplate()
    {
        //добавить диалоговое окно
        db.AddTemplate("C:\\Users\\User\\Desktop\\работы\\проект\\teamwork-positive\\Documents\\Договорные документы\\Договор аренды квартиры.docx", SelectedCategory);
        Templates = db.GetTemplates(SelectedCategory);
    }
    //public void FillSelected ()
    //{
    //    FillViewModel fillViewModel = new FillViewModel(new Backend(), SelectedTemplate);
    //}
}
