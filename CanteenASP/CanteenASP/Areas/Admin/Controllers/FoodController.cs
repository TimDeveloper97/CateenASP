using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace CanteenASP.Areas.Admin.Controllers
{
    public class FoodController : Controller
    {
        ICrud<Food> _iFoodService;

        public FoodController(ICrud<Food> iFoodService)
        {
            this._iFoodService = iFoodService;
        }

        public IActionResult Index()
        {


            return View();
        }
    }
}
