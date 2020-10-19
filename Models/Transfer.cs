using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TestNikita.Models
{
  public class Transfer : CreateTransfer
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime DocDate { get; set; }

    public string UserId { get; set; }
  }
}