using API;
using API.Helper;
using CanteenASP.Models;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Diagnostics;
using System.Globalization;


namespace CanteenASP.Controllers
{
    public class HomeController : BaseController
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
            if (userId != null)
            {
                var orders = await _orderService.GetOrdersByUser(userId);
                ViewBag.Orders = orders.Where(x => x.OrderTime.Date == DateTime.Now.Date).FirstOrDefault();
            }
            foreach (var item in foods)
            {
                item.Price = double.Parse(item.Price).ToString("#,###", cul.NumberFormat);
            }
            ViewData["Message"] = TempData["Message"];
            ViewData["Flag"] = TempData["Flag"];  
            return View(foods);
        }

        public async Task<IActionResult> Meals(int meal)
        {
            var foods = await _foodService.GetFoodByMealTime((MealTime)meal);
            foods = foods.Where(x => x.Type == (Model.Type)2).ToList();
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            foreach (var item in foods)
            {
                item.Price = double.Parse(item.Price).ToString("#,###", cul.NumberFormat);
            }
            if(meal == 1)
            {
                ViewBag.Meal = "Breakfast";
            }
            else if(meal == 2)
            {
                ViewBag.Meal = "Lunch";
            }
            else
            {
                ViewBag.Meal = "Dinner";
            }    
            return View(foods);
        }

        public async Task<IActionResult> Details(string id)
        {
            var food = await _foodService.GetItem(id);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            food.Price = double.Parse(food.Price).ToString("#,###", cul.NumberFormat);
            return View(food);
        }

        public async Task<IActionResult> OptionDetails(int meal)
        {
            var foods = await _foodService.GetFoodByMealTime((MealTime)meal);
            foods = foods.Where(x => x.Type == (Model.Type)1).ToList();
            var details = new List<string>();
            foreach(var item in foods)
            {
                details.Add(item.Detail);
            }
            ViewData["details"] = details;
            return View(foods);
        }
        [HttpPost]
        public async Task<IActionResult> Order([FromBody] OrderPayload orderPayload)
        {
            

            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return Json(new { redirectToUrl = Url.Action("Index", "User") });
            }
            var user = await _userService.GetItem(userId);
            var food = await _foodService.GetItem(orderPayload.Id);
            food.Size = (Size)int.Parse(orderPayload.Size);
            var orderTime = DateTime.Now;
            var flag = await _orderService.MealTimeIsExist(userId, food.MealTime);
            
            if (!flag)
            {
                var order = new Order()
                {
                    User = user,
                    Food = food,
                    OrderTime = orderTime,
                    TotalPrice = (float.Parse(food.Price) + ((float)food.Size - 1) * 5000).ToString()
                };
                var res = await _orderService.Create(order);
                if (!res)
                    TempData["Message"] = "Time to order this meal is up!";
            }
            else
                TempData["Message"] = "This meal has already ordered today!";

            return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }

        [HttpPost]
        public async Task<IActionResult> OptionOrder([FromBody] List<OrderPayload> orderPayloads)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return Json(new { redirectToUrl = Url.Action("Index", "User") });
            }
            var user = await _userService.GetItem(userId);
            var orderTime = DateTime.Now;
            List<Food> foods = new List<Food>();
            var totalPrice = 0;
            foreach(var item in orderPayloads)
            {
                var food = await _foodService.GetItem(item.Id);
                food.Size = (Size)int.Parse(item.Size);
                var price = int.Parse(food.Price) + ((int)food.Size - 1) * 5000;
                totalPrice += price;
                foods.Add(food);
            }
            var flag = await _orderService.MealTimeIsExist(userId, foods);
            if (!flag)
            {
                var order = new Order()
                {
                    User = user,
                    Foods = foods,
                    OrderTime = orderTime,
                    TotalPrice = totalPrice.ToString()
                };
                var res = await _orderService.Create(order);
                if (!res)
                    TempData["Message"] = "Time to order this meal is up!";
            }   
            else
                TempData["Message"] = "This meal has already ordered today!";
            return Json(new { redirectToUrl = Url.Action("Index", "Home") });
        }

        public async Task<IActionResult> CancelOrder(string id)
        {
            var res = await _orderService.Delete(id);
            if (res)
            {
                TempData["Message"] = "Canceled order successfully!";
                return RedirectToAction("Index");
            }
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