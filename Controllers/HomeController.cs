using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestNikita.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

namespace TestNikita.Controllers
{
  [ApiController]
  [Route("api/p2p/transfer")]
  public class HomeController : RootController
  {

    public HomeController(DataService context) : base(context) { }

    [Authorize(Roles = "user")]
    [HttpGet("history")]
    public async Task<CommonFormat<IEnumerable<Transfer>>> GetAllTransfers(GetHistory getHistory)
    {
      var result = await db.getTransfers(getHistory);
      var model = new CommonFormat<IEnumerable<Transfer>> { Error = "", Success = true, Data = result };

      return model;
    }

    [Authorize(Roles = "user")]
    [HttpGet("history/{id}")]
    public async Task<CommonFormat<Transfer>> GetTransfer(GetOneHistory getOneHistory)
    {
      var result = await db.GetTransfer(getOneHistory);

      return new CommonFormat<Transfer> { Error = "", Success = true, Data = result };
    }

    [Authorize(Roles = "user")]
    [HttpPost("create")]
    public async Task<CommonFormat<Transfer>> CreateOne(CreateTransfer createTransfer)
    {
      Transfer transfer = new Transfer
      {
        ExpiryMonth = createTransfer.ExpiryMonth,
        ExpiryYear = createTransfer.ExpiryYear,
        SenderCardNumber = createTransfer.SenderCardNumber,
        RecipientCardNumber = createTransfer.RecipientCardNumber,
        FullName = createTransfer.FullName,
        DocDate = DateTime.Now,
        Sum = createTransfer.Sum
      };

      await db.CreateTransfer(transfer);

      return new CommonFormat<Transfer> { Error = "", Success = true, Data = transfer };
    }

    [Authorize(Roles = "user")]
    [HttpDelete("delete/{id}")]
    public async Task<CommonFormat<string>> DeleteOne(string Id)
    {
      await db.Remove(Id);

      return new CommonFormat<string> { Error = "", Success = true, Data = "success" };
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("delete/all")]
    public async Task<CommonFormat<string>> DeleteAll()
    {
      await db.RemoveAll();

      return new CommonFormat<string> { Error = "", Success = true, Data = "success" };
    }
  }
}