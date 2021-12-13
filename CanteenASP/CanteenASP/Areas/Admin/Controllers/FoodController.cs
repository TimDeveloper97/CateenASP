using Microsoft.AspNetCore.Mvc;

namespace CanteenASP.Areas.Admin.Controllers
{
    public class FoodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
