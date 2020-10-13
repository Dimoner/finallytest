using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestNikita.Models
{
  public class CreateUser
  {
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Display(Name = "Имя")]
    public string Name { get; set; }

    [Display(Name = "Пароль")]
    public string Password { get; set; }
  }
}