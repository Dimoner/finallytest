using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestNikita.Models
{
  public class CreateUser
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }
  }
}