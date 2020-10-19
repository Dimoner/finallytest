namespace TestNikita.Models
{
  public class CreateTransfer
  {
    public int ExpiryMonth { get; set; }

    public int ExpiryYear { get; set; }

    public string SenderCardNumber { get; set; }

    public string RecipientCardNumber { get; set; }

    public string FullName { get; set; }

    public double Sum { get; set; }
  }
}