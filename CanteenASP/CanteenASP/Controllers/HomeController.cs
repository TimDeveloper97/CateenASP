using API;
using CanteenASP.Models;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Diagnostics;
using System.Globalization;

namespace CanteenASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FoodService _foodService;
        private readonly UserService _userService;
        private readonly OrderService _orderService;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _foodService = new FoodService();
            _userService = new UserService();
            _orderService = new OrderService();
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _foodService.GetAll();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var userId = HttpContext.Session.GetString("UserId");
            if(userId != null)
            {
                ViewBag.Orders = await _orderService.GetOrdersByUser(userId);
            }
            foreach(var item in foods)
            {
                item.Price = double.Parse(item.Price).ToString("#,###", cul.NumberFormat);
            }
            ViewData["Message"] = TempData["Message"];
            ViewData["Flag"] = TempData["Flag"];
            return View(foods);
        }

        public async Task<IActionResult> Details(string id)
        {
            var food = await _foodService.GetItem(id);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            food.Price = double.Parse(food.Price).ToString("#,###",cul.NumberFormat);
            return View(food);
        }

        public async Task<IActionResult> Order(string id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if(userId == null)
            {
                return RedirectToAction("Index", "User");
            }
            var user = await _userService.GetItem(userId);
            var food = await _foodService.GetItem(id);
            var orderTime = DateTime.Now;
            var order = new Order()
            {
                User = user,
                Food = food,
                OrderTime = orderTime,
            };
            var res = await _orderService.Create(order);
            if (res)
            {
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Time to order this meal is up!";
            TempData["Flag"] = 1;
            return RedirectToAction("Index");
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