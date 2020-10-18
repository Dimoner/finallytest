using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestNikita.Models;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
      bool isNotValid = !Helpers.Function.ValidString(user.Name) || !Helpers.Function.ValidString(user.Password);

      if (isNotValid)
      {
        return BadRequest(new CommonFormat<object> { Error = "Не заполнены поля", Success = false, Data = null });
      }

      try
      {
        var registerAccount = await db.CreateUser(user);

        if (registerAccount == null)
        {
          return BadRequest(new CommonFormat<object> { Error = "Логин занят", Success = false, Data = null });
        }

        return Json(new CommonFormat<string> { Error = "", Success = true, Data = "Аккаунт успешно создан" });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }

    }

    [Authorize]
    [HttpGet("account")]
    public async Task<IActionResult> AccountInfo()
    {
      try
      {
        User person = await db.GetUser(User.Identity.Name);

        AccountInfo accountInfo = new AccountInfo { Name = person.Name, Role = person.Role };

        return Json(new CommonFormat<AccountInfo> { Error = "", Success = true, Data = accountInfo });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Token(Auth authData)
    {
      bool isNotValid = !Helpers.Function.ValidString(authData.Name) || !Helpers.Function.ValidString(authData.Password);

      if (isNotValid)
      {
        return BadRequest(new CommonFormat<object> { Error = "Не заполнены поля", Success = false, Data = null });
      }

      try
      {
        User person = await db.GetUser(authData.Name, authData.Password);

        if (person == null)
        {
          return BadRequest(new CommonFormat<string> { Error = "Неправильный логин или пароль", Success = false, Data = null });
        }

        var identity = Helpers.Function.GetIdentity(person);

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

        var resp = new
        {
          access_token = encodedJwt,
          userId = identity.Name
        };

        var response = new CommonFormat<object> { Error = "", Success = true, Data = resp };

        return Json(response);
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }
    }
  }
}