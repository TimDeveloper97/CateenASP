using API;
using Microsoft.AspNetCore.Mvc;

namespace CanteenASP.Controllers.Components
{
    public class UserInforViewComponent : ViewComponent
    {
        UserService _userService = new UserService(); 
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            
            if(userId == null)
            {
                Model.User user = null;
                return View("Default", user);
            }
            else
            {
                var user = await _userService.GetItem(userId);
                return View("Default", user);
            }
        }
    }
}
