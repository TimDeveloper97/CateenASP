using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CanteenASP.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            var role = context.HttpContext.Session.GetString("Role");
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Index", "User", new { area = "" });
            }
            if(role != "Admin")
            {
                context.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
