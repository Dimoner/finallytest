using System.ComponentModel.DataAnnotations;

namespace TestNikita.Models
{
  public class GetOneHistory : GetHistory
  {
    [Display(Name = "Id записи")]
    public string TransferId { get; set; }
  }
}