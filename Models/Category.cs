using System.Collections.ObjectModel;
using ReactiveUI;
using LiteDB;

namespace Documently.Models;

public class Category : ReactiveObject
{
    private string _name;
    private int _count;

    [BsonId]
    public ObjectId Id { get; }
    public ObjectId Parent { get; set; }
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    [BsonIgnore]
    public int Count
    {
        get => _count;
        set => this.RaiseAndSetIfChanged(ref _count, value);
    }

    [BsonIgnore]
    public ObservableCollection<Category> Children { get; }

    [BsonCtor]
    /* This thing turned out to be sensitive to argument names */
    public Category (ObjectId id, ObjectId parent, string name)
    {
        Id = id;
        Parent = parent;
        _name = name;
        Children = new ObservableCollection<Category>();
    }

    /* This constructor is intended to be used for root */
    public Category (string name)
    {
        Id = ObjectId.NewObjectId();
        Parent = ObjectId.Empty;
        _name = name;
        Children = new ObservableCollection<Category>();
    }

    public Category (string name, Category parent)
    {
        Id = ObjectId.NewObjectId();
        Parent = parent.Id;
        _name = name;
        Children = new ObservableCollection<Category>();

        /* Automatically attach new category to the tree */
        parent.Children.Add(this);
    }
}
