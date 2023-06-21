using System.Collections.ObjectModel;
using LiteDB;

namespace Documently.Models;

public class Category
{
    [BsonId]
    public ObjectId Id { get; }
    public ObjectId Parent { get; set; }
    public string Name { get; set; }

    [BsonIgnore]
    public ObservableCollection<Category> Children { get; }

    [BsonCtor]
    /* This thing turned out to be sensitive to argument names */
    public Category (ObjectId id, ObjectId parent, string name)
    {
        Id = id;
        Parent = parent;
        Name = name;
        Children = new ObservableCollection<Category>();
    }

    /* This constructor is intended to be used for root */
    public Category (string name)
    {
        Id = ObjectId.NewObjectId();
        Parent = ObjectId.Empty;
        Name = name;
        Children = new ObservableCollection<Category>();
    }

    public Category (string name, Category parent)
    {
        Id = ObjectId.NewObjectId();
        Parent = parent.Id;
        Name = name;
        Children = new ObservableCollection<Category>();

        /* Automatically attach new category to the tree */
        parent.Children.Add(this);
    }
}
