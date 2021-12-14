using API;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace CanteenASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : Controller
    {
        FoodService _foodService;

        public FoodController()
        {
            this._foodService = new FoodService();
        }

        public async Task<IActionResult> Index()
        {
            var lFood = await _foodService.GetAll();
            return View(lFood);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Food food)
        {
            var result = await _foodService.Create(food);
            if(result)
            {
                return RedirectToAction("Index");
            }
            return View(food);
        }

        public IActionResult Delete()
        {


            return View();
        }
    }
}
