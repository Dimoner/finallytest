using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestNikita.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestNikita;

namespace TestNikita.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserController : HomeController
  {
    public UserController(DataService context) : base(context) { }

    [HttpPost("createUser")]
    public async Task<CommonFormat<string>> CreateUser(CreateUser user)
    {
      await db.CreateUser(user);

      return new CommonFormat<string> { Error = "", Success = true, Data = "success" };
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Token(Auth authData)
    {
      User person = await db.GetUser(authData.Name, authData.Password);

      if (person == null)
      {
        return BadRequest(new CommonFormat<string> { Error = "Неправильный логин или пароль", Success = false, Data = null });
      }

      var identity = GetIdentity(person);

      if (identity == null)
      {
        return BadRequest(new CommonFormat<string> { Error = "Неправильный логин или пароль", Success = false, Data = null });
      }

      var now = DateTime.UtcNow;
      // создаем JWT-токен
      var jwt = new JwtSecurityToken(
              issuer: AuthOption.ISSUER,
              audience: AuthOption.AUDIENCE,
              notBefore: now,
              claims: identity.Claims,
              expires: now.Add(TimeSpan.FromMinutes(AuthOption.LIFETIME)),
              signingCredentials: new SigningCredentials(AuthOption.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
      var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

      var response = new
      {
        access_token = encodedJwt,
        username = identity.Name,
        userId = person.Id
      };

      return Json(response);
    }

    private ClaimsIdentity GetIdentity(User person)
    {
      if (person != null)
      {
        var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return claimsIdentity;
      }


      return null;
    }
  }
}