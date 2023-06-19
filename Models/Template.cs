using LiteDB;

namespace Documently.Models;

public class Template
{
    [BsonId]
    public ObjectId Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }

    [BsonRef("categories")]
    public Category Tag { get; set; }

    [BsonCtor]
    public Template (ObjectId id, string name, string description, BsonDocument tag)
    {
        Id = id;
        Name = name;
        Description = description;
        Tag = BsonMapper.Global.Deserialize<Category>(tag);
    }

    public Template (string name, string description, Category tag)
    {
        Id = ObjectId.NewObjectId();
        Name = name;
        Description = description;
        Tag = tag;
    }
}
