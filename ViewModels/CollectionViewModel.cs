using System.Collections.ObjectModel;
using System.Collections.Generic;
using ReactiveUI;
using Documently.Models;

namespace Documently.ViewModels;

public class CollectionViewModel : ViewModelBase
{
    public TemplateDatabase db;
    Category selectedCategory;
    List<Template> templates;
    Template selectedTemplate;

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
    }
    ~CollectionViewModel()
    {
        db.Close();
    }
    public void AddCategory(string name)
    {
        db.AddCategory(name);
    }
    public void AddSubCategory(string name)
    {
        db.AddSubCategory(SelectedCategory, name);
    }
    public void RenameCategory(string name)
    {
        db.RenameCategory(SelectedCategory, name);
    }
    public void RemoveCategory()
    {
        db.RemoveCategory(SelectedCategory);
    }
    public void UploadTemplate(string path)
    {
        db.AddTemplate(path, SelectedCategory);
        Templates = db.GetTemplates(SelectedCategory);
    }
    public void RemoveTemplate()
    {
        db.RemoveTemplate(SelectedTemplate);
        Templates = db.GetTemplates(SelectedCategory);
    }
}
