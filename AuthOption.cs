using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestNikita
{
  public class AuthOption
  {
    public const string ISSUER = "TestNikitaServer";
    public const string AUDIENCE = "TestNikitaFront";
    const string KEY = "ftithebestsharaga";
    public const int LIFETIME = 300;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
      return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
  }
}