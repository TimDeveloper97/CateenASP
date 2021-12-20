using API;
using CanteenASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace CanteenASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FoodService _foodService;
        

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _foodService = new FoodService();
            
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _foodService.GetAll();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            foreach(var item in foods)
            {
                item.Price = double.Parse(item.Price).ToString("#,###", cul.NumberFormat);
            }
            return View(foods);
        }

        public async Task<IActionResult> Details(string id)
        {
            var food = await _foodService.GetItem(id);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            food.Price = double.Parse(food.Price).ToString("#,###",cul.NumberFormat);
            return View(food);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}