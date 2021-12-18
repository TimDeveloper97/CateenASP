using API;
using CanteenASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CanteenASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FoodService _foodService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _foodService = new FoodService();
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _foodService.GetAll();
            return View(foods);
        }

        public async Task<IActionResult> Details(string id)
        {
            var food = await _foodService.GetItem(id);
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