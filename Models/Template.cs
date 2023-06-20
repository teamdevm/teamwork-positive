using LiteDB;

namespace Documently.Models;

public class Template
{
    [BsonId]
    public ObjectId Id { get; }
    public ObjectId Category { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [BsonCtor]
    public Template (ObjectId id, ObjectId category, string name, string description)
    {
        Id = id;
        Category = category;
        Name = name;
        Description = description;
    }

    public Template (string name, string description, Category tag)
    {
        Id = ObjectId.NewObjectId();
        Category = tag.Id;
        Name = name;
        Description = description;
    }
}
