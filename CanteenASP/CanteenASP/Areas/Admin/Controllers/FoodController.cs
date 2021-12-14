using API;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;
using MongoDB.Bson;

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
            food.Id = ObjectId.GenerateNewId();
            var result = await _foodService.Create(food);
            if(result)
            {
                return RedirectToAction("Index");
            }
            return View(food);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var food = await _foodService.Read(new ObjectId(id));
            return View(food);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Food food)
        {
            var result = await _foodService.Update(food);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(food);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _foodService.Delete(new ObjectId(id));
            return RedirectToAction("Index");
        }
    }
}
