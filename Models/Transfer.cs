using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestNikita.Models
{
  public class Transfer : CreateTransfer
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime DocDate { get; set; }

    [Display(Name = "Id пользователя")]
    public string UserId { get; set; }

  }
}