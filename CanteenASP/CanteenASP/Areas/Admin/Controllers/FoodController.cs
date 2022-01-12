using API;
using API.Interface;
using CanteenASP.Controllers;
using Microsoft.AspNetCore.Mvc;
using Model;
using MongoDB.Bson;

namespace CanteenASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodController : BaseController
    {
        readonly FoodService _foodService;
        string _pWwwRoot;

        public FoodController(IHostEnvironment webHostEnviroment)
        {
            this._foodService = new FoodService();
            _pWwwRoot = Path.Combine(webHostEnviroment.ContentRootPath, "wwwroot");
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(Food food, List<IFormFile> postedFiles)
        {
            if(string.IsNullOrEmpty(food.Name) 
                || string.IsNullOrEmpty(food.Price)
                || string.IsNullOrEmpty(food.SideDishes)
                || string.IsNullOrEmpty(food.Detail))
                return View(food);

            string path = Path.Combine(_pWwwRoot, "assets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (postedFiles.Count != 0)
            {
                var filename = DateTime.Now.ToString("HHmmss_ddMMyyyy") + ".png";
                food.Image = filename;
                using (FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    postedFiles[0].CopyTo(stream);
                }
            }

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
            var food = await _foodService.GetItem(id);
            return View(food);
        }

        [HttpPost]
        [Obsolete]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(Food food, List<IFormFile> postedFiles)
        {
            if (string.IsNullOrEmpty(food.Name)
                || string.IsNullOrEmpty(food.Price)
                || string.IsNullOrEmpty(food.SideDishes))
                return View(food);

            string path = Path.Combine(_pWwwRoot, "assets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (postedFiles.Count != 0)
            {
                if(!string.IsNullOrEmpty(food.Image))
                {
                    System.IO.File.Delete(Path.Combine(path, food.Image));
                }

                var filename = DateTime.Now.ToString("HHmmss_ddMMyyyy") + ".png";
                food.Image = filename;
                using (FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    postedFiles[0].CopyTo(stream);
                }
            }

            var result = await _foodService.Update(food);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(food);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _foodService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
