using System.Collections.Generic;

namespace TestNikita.Models
{
  public class CommonFormat<T>
  {
    public string Error { get; set; }

    public bool Success { get; set; }

    public T Data { get; set; }
  }
}