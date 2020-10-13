using System.ComponentModel.DataAnnotations;

namespace TestNikita.Models
{
  public class User : CreateUser
  {
    [Display(Name = "Роль")]
    public string Role { get; set; }
  }
}