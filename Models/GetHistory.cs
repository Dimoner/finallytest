using System.ComponentModel.DataAnnotations;

namespace TestNikita.Models
{
  public class GetHistory
  {
    [Display(Name = "Id пользователя")]
    public string UserId { get; set; }
  }
}