using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestNikita.Models;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TestNikita.Controllers
{

  [ApiController]
  [Route("api/p2p/transfer")]
  public class HomeController : RootController
  {

    public HomeController(DataService context) : base(context) { }

    [Authorize(Roles = AppOption.COMMON_METHOD)]
    [HttpGet("history")]
    public async Task<IActionResult> GetAllTransfers()
    {
      string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

      try
      {
        var result = await db.getTransfers(User.Identity.Name, role);

        return Json(new CommonFormat<IEnumerable<Transfer>> { Error = "", Success = true, Data = result });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }

    }

    [Authorize(Roles = AppOption.COMMON_METHOD)]
    [HttpGet("history/{transferId}")]
    public async Task<IActionResult> GetTransfer(string transferId)
    {
      if (!Helpers.Function.ValidString(transferId))
      {
        return BadRequest(new CommonFormat<object> { Error = "Не корректный id записи", Success = false, Data = null });
      }

      string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

      try
      {
        var result = await db.GetTransfer(User.Identity.Name, role, transferId);

        if (result == null)
        {
          return BadRequest(new CommonFormat<object> { Error = "Запись не найдена", Success = false, Data = null });
        }

        return Json(new CommonFormat<Transfer> { Error = "", Success = true, Data = result });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }
    }

    [Authorize(Roles = AppOption.COMMON_METHOD)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateOne(CreateTransfer createTransfer)
    {
      string validation = Helpers.Function.ValidationCreatTransfer(createTransfer);

      if (Helpers.Function.ValidString(validation))
      {
        return BadRequest(new CommonFormat<object> { Error = validation, Success = false, Data = null });
      }

      Transfer transfer = new Transfer
      {
        ExpiryMonth = createTransfer.ExpiryMonth,
        ExpiryYear = createTransfer.ExpiryYear,
        SenderCardNumber = createTransfer.SenderCardNumber,
        RecipientCardNumber = createTransfer.RecipientCardNumber,
        FullName = createTransfer.FullName,
        DocDate = DateTime.Now,
        Sum = createTransfer.Sum,
        UserId = User.Identity.Name
      };

      try
      {
        await db.CreateTransfer(transfer);

        return Json(new CommonFormat<Transfer> { Error = "", Success = true, Data = transfer });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }
    }

    [Authorize(Roles = AppOption.COMMON_METHOD)]
    [HttpDelete("delete/{transferId}")]
    public async Task<IActionResult> DeleteOne(string transferId)
    {

      if (!Helpers.Function.ValidString(transferId))
      {
        return BadRequest(new CommonFormat<object> { Error = "Не корректный id записи", Success = false, Data = null });
      }

      string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

      try
      {
        await db.Remove(User.Identity.Name, role, transferId);

        return Json(new CommonFormat<string> { Error = "", Success = true, Data = "success" });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }

    }

    [Authorize(Roles = AppOption.ADMIN_ROLE)]
    [HttpDelete("delete/all")]
    public async Task<IActionResult> DeleteAll()
    {
      try
      {
        await db.RemoveAll();

        return Json(new CommonFormat<string> { Error = "", Success = true, Data = "success" });
      }
      catch
      {
        return BadRequest(new CommonFormat<object> { Error = "База сломалась", Success = false, Data = null });
      }

    }

  }
}