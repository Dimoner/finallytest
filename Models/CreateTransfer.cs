using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestNikita.Models
{
  public class CreateTransfer
  {
    [Display(Name = "Месяц истечения")]
    public int ExpiryMonth { get; set; }

    [Display(Name = "Год истечения")]
    public int ExpiryYear { get; set; }

    [Display(Name = "Номер карты отправителя")]
    public string SenderCardNumber { get; set; }

    [Display(Name = "Номер карты получателя")]
    public string RecipientCardNumber { get; set; }

    [Display(Name = "Имя")]
    public string FullName { get; set; }

    [Display(Name = "Сумма")]
    public int Sum { get; set; }
  }
}