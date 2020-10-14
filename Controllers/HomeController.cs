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
    public async Task<CommonFormat<IEnumerable<Transfer>>> GetAllTransfers()
    {
      var result = await db.getTransfers(ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

      var model = new CommonFormat<IEnumerable<Transfer>> { Error = "", Success = true, Data = result };

      return model;
    }

    [Authorize(Roles = AppOption.COMMON_METHOD)]
    [HttpGet("history/{transferId}")]
    public async Task<IActionResult> GetTransfer(string transferId)
    {
      if (!Helpers.Function.ValidString(transferId))
      {
        return BadRequest(new CommonFormat<object> { Error = "Не корректный id записи", Success = false, Data = null });
      }

      var result = await db.GetTransfer(ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType, transferId);

      if (result == null)
      {
        return BadRequest(new CommonFormat<object> { Error = "Запись не найдена", Success = false, Data = null });
      }

      return Json(new CommonFormat<Transfer> { Error = "", Success = true, Data = result });
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
        UserId = ClaimsIdentity.DefaultNameClaimType
      };

      await db.CreateTransfer(transfer);

      return Json(new CommonFormat<Transfer> { Error = "", Success = true, Data = transfer });
    }

    [Authorize(Roles = AppOption.COMMON_METHOD)]
    [HttpDelete("delete/{transferId}")]
    public async Task<IActionResult> DeleteOne(string transferId)
    {
       
      if (!Helpers.Function.ValidString(transferId))
      {
        return BadRequest(new CommonFormat<object> { Error = "Не корректный id записи", Success = false, Data = null });
      }

      await db.Remove(ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType, transferId);

      return Json(new CommonFormat<string> { Error = "", Success = true, Data = "success" });
    }

    [Authorize(Roles = AppOption.ADMIN_ROLE)]
    [HttpDelete("delete/all")]
    public async Task<CommonFormat<string>> DeleteAll()
    {
      await db.RemoveAll();

      return new CommonFormat<string> { Error = "", Success = true, Data = "success" };
    }

  }
}