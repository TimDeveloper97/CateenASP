using Microsoft.AspNetCore.Mvc;

namespace CanteenASP.Controllers
{
    public class SigninController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
