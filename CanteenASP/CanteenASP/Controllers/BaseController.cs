using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CanteenASP.Controllers
{
    public class BaseController : Controller
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            
            if(userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "User", new {area = ""});
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
