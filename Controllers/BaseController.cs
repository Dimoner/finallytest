using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestNikita.Models;

public abstract class BaseController : Controller
{
  public override void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.HttpContext.Request.Headers.ContainsKey("User-Agent"))
    {
      var userAgent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();

      if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
      {
        string ErrorText = "Это какая-то утерянная древняя технология, мы с таким не работаем(((";

        context.Result = Json(new CommonFormat<string> { Error = ErrorText, Success = false, Data = null });
      }
    }

    base.OnActionExecuted(context);
  }
}