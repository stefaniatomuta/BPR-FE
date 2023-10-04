using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BPRBE.Models.Persistence;

public class Rule
{
    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}