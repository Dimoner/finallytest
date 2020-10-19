using TestNikita.Models;
using System.Security.Claims;
using System.Collections.Generic;


namespace TestNikita.Helpers
{
  public static class Function
  {
    public static bool ValidString(string str)
    {
      return !string.IsNullOrEmpty(str);
    }

    public static string ValidationCreatTransfer(CreateTransfer createTransfer)
    {
      string validation = "";

      if (createTransfer.ExpiryMonth < 1 || createTransfer.ExpiryMonth > 12)
      {
        validation += "Неверно указан месяц. ";
      }
      else if (createTransfer.ExpiryYear < 20)
      {
        validation += "Неверно указан год. ";
      } 
      else if (!Function.ValidString(createTransfer.FullName))
      {
        validation += "Укажите имя "; 
      }
      else if (!Function.ValidString(createTransfer.RecipientCardNumber) || createTransfer.RecipientCardNumber.Length != 16)
      {
        validation += "Неверно указана карта принимающей стороны. ";
      }
      else if (!Function.ValidString(createTransfer.SenderCardNumber) || createTransfer.SenderCardNumber.Length != 16)
      {
        validation += "Неверно указана карта отправителя. ";
      }
      else if (createTransfer.Sum < 0)
      {
        validation += "Сумма должна быть больше 0. ";
      }

      return validation.Trim();
    }

    public static ClaimsIdentity GetIdentity(User person)
    {
      var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Id),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };

      ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
          ClaimsIdentity.DefaultRoleClaimType);

      return claimsIdentity;
    }
  }
}