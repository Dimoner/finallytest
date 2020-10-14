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
using Microsoft.AspNetCore.Authorization;

namespace TestNikita.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class UserController : HomeController
  {
    public UserController(DataService context) : base(context) { }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser(CreateUser user)
    {
      bool isNotValid = string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password);

      if (isNotValid)
      {
        return BadRequest(new CommonFormat<object> { Error = "Не заполнены поля", Success = false, Data = null });
      }

      var registerAccount = await db.CreateUser(user);

      if (registerAccount == null)
      {
        return BadRequest(new CommonFormat<object> { Error = "Логин занят", Success = false, Data = null });
      }

      return Json(new CommonFormat<string> { Error = "", Success = true, Data = "Аккаунт успешно создан" });
    }

    [Authorize]
    [HttpGet("account")]
    public async Task<CommonFormat<AccountInfo>> AccountInfo()
    {
      User person = await db.GetUser(ClaimsIdentity.DefaultNameClaimType);

      AccountInfo accountInfo = new AccountInfo { Name = person.Name, Role = person.Role };

      return new CommonFormat<AccountInfo> { Error = "", Success = true, Data = accountInfo };
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Token(Auth authData)
    {
      bool isNotValid = string.IsNullOrEmpty(authData.Name) || string.IsNullOrEmpty(authData.Password);

      if (isNotValid)
      {
        return BadRequest(new CommonFormat<object> { Error = "Не заполнены поля", Success = false, Data = null });
      }

      User person = await db.GetUser(authData.Name, authData.Password);

      if (person == null)
      {
        return BadRequest(new CommonFormat<string> { Error = "Неправильный логин или пароль", Success = false, Data = null });
      }

      var identity = GetIdentity(person);

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
        userId = identity.Name
      };

      return Json(response);
    }

    private ClaimsIdentity GetIdentity(User person)
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