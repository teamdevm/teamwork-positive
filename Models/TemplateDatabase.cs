using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System;
using LiteDB;

namespace Documently.Models;

public class TemplateDatabase
{
    private LiteDatabase db;
    private ILiteCollection<Category> categories;
    private ILiteCollection<Template> templates;
    private ILiteStorage<ObjectId> storage;
    private Category root;
    private Category none;

    public ObservableCollection<Category> Categories
    {
        get => root.Children;
    }

    public List<Category> CategoriesAsList
    {
        get => categories.Query()
            .Where(x => x.Parent != ObjectId.Empty)
            .OrderBy(x => x.Name)
            .ToList();
    }

    public TemplateDatabase (string path)
    {
        db = new LiteDatabase(path);
        categories = db.GetCollection<Category>("categories");
        templates = db.GetCollection<Template>("templates");
        storage = db.GetStorage<ObjectId>();

        /* Setup indexes for faster lookup */
        categories.EnsureIndex(x => x.Parent);
        categories.EnsureIndex(x => x.Name);

        /* Setup the system category 'root' */

        root = categories.FindOne(
            x => (x.Parent == ObjectId.Empty) && (x.Name == "root")
        );

        if (root is null)
        {
            root = new Category("root");
            categories.Insert(root);
        }

        /* Setup the system category 'none' */

        none = categories.FindOne(
            x => (x.Parent == ObjectId.Empty) && (x.Name != "root")
        );

        if (none is null)
        {
            none = new Category("none");
            categories.Insert(none);
        }

        /* Internal representation of the tree is a little tricky */
        DeserializeCategoryTree(root);

        /* 'none' category should be shown on the UI */
        root.Children.Add(none);
        /* Templates in 'none' category should be counted separately */
        none.Count = templates.Query().Where(x => x.Category == none.Id).Count();
    }

    public void Close ()
    {
        db.Dispose();
    }

    public void AddCategory (string name) => AddSubCategory(root, name);

    public void AddSubCategory (Category target, string name)
    {
        if (target == none)
        {
            throw new ArgumentException("Для данной категории не поддерживается добавление дочерних категорий");
        }

        Category a = categories.FindOne(x => x.Name == name);

        if (a is null)
        {
            Category c = new Category(name, target);
            categories.Insert(c);
        }
        else
        {
            throw new ArgumentException($"Категория с именем '{name}' уже существует");
        }
    }

    public void RenameCategory (Category target, string newname)
    {
        Category a = categories.FindOne(x => x.Name == newname);

        if (a is null)
        {
            target.Name = newname;
            categories.Update(target);
        }
        else
        {
            throw new ArgumentException($"Категория с именем '{newname}' уже существует");
        }
    }

    public void RemoveCategory (Category target)
    {
        if (target.Parent == ObjectId.Empty)
        {
            throw new ArgumentException("Удаление системных категорий недопустимо");
        }

        DFSRemoveCategory(root, target);
        categories.Delete(target.Id);
    }

    public void AddTemplate (string path) => AddTemplate(path, none);

    public void AddTemplate (string path, Category tag)
    {
        string pathToXml = Converter.ToXml(path);
        string name = Path.GetFileName(pathToXml);
        Template t = new Template(name, "", tag);
        
        storage.Upload(t.Id, pathToXml);
        templates.Insert(t);

        tag.Count += 1;

        if (File.Exists(pathToXml))
        {
            File.Delete(pathToXml);
        }
    }

    public void RenameTemplate (Template t, string newname)
    {
        t.Name = newname;
        templates.Update(t);
    }

    public void RemoveTemplate (Template t)
    {
        templates.Delete(t.Id);
        storage.Delete(t.Id);

        Category c = FindCategoryById(root, t.Category);
        c.Count -= 1;
    }

    public List<Template> GetTemplates () => GetTemplates(root);

    public List<Template> GetTemplates (Category c)
    {
        return templates.Query()
            .Where(x => x.Category == c.Id)
            .OrderBy(x => x.Name)
            .ToList();
    }

    public void SetTemplateCategory (Template t, Category c)
    {
        t.Category = c.Id;
        c.Count += 1;
        templates.Update(t);
    }

    public LiteFileInfo<ObjectId> GetTemplateFileInfo (Template t)
    {
        return storage.FindById(t.Id);
    }

    public MemoryStream FetchTemplate (Template t)
    {
        MemoryStream s = new MemoryStream();
        storage.Download(t.Id, s);
        return s;
    }

    private void DeserializeCategoryTree (Category c)
    {
        var a = categories.Query().Where(x => x.Parent == c.Id).OrderBy(x => x.Name).ToEnumerable();
        c.Count = templates.Query().Where(x => x.Category == c.Id).Count();

        foreach (Category d in a)
        {
            c.Children.Add(d);
            DeserializeCategoryTree(d);
        }
    }

    private void ResetTemplateCategory (Category c)
    {
        foreach (Template t in GetTemplates(c))
        {
            t.Category = none.Id;
            none.Count += 1;
            templates.Update(t);
        }
    }

    private Category FindCategoryById (Category c, ObjectId id)
    {
        if (c.Id == id) return c;
        Category e;

        foreach (Category d in c.Children)
        {
            e = FindCategoryById(d, id);
            if (e is not null) return e;
        }

        return null!;
    }

    private bool DFSRemoveCategory (Category cursor, Category target)
    {
        if (cursor.Children.Remove(target))
        {
            /* cursor here is the parent of the removed directory */

            foreach (Category child in target.Children)
            {
                /* Adopt children of the removed category */
                cursor.Children.Add(child);
                /* Update parent of the orphans */
                child.Parent = target.Parent;
                /* Update them in the database too */
                categories.Update(child);
            }

            /* Reset the category of all templates referencing this category */
            ResetTemplateCategory(target);

            return true;
        }

        foreach (Category e in cursor.Children)
        {
            if (DFSRemoveCategory(e, target)) return true;
        }

        return false;
    }
}