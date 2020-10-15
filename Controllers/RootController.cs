using Microsoft.AspNetCore.Mvc;
using TestNikita.Models;

namespace TestNikita.Controllers
{
  [ApiController]
  public class RootController : BaseController
  {
    public readonly DataService db;

    public RootController(DataService context)
    {
      db = context;
    }
  }
}