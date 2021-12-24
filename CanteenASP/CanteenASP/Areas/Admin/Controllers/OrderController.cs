using API;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace CanteenASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        readonly OrderService _orderService;
        string _pWwwRoot;
        static List<Order> _tmpOrders;

        public OrderController(IHostEnvironment webHostEnviroment)
        {
            this._orderService = new OrderService();
            _tmpOrders = new List<Order>();
            _pWwwRoot = Path.Combine(webHostEnviroment.ContentRootPath, "wwwroot");
        }

        public async Task<IActionResult> Index(string mealTime, string startTime, string endTime)
        {
            var lOrder = await _orderService.GetAll();
            if(!string.IsNullOrEmpty(mealTime))
                lOrder = lOrder.Where(x => x.Food.MealTime.ToString() == mealTime).ToList();

            if (!string.IsNullOrEmpty(startTime))
            {
                var start = DateTime.ParseExact(startTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                lOrder = lOrder.Where(x => x.OrderTime >= start).ToList();
            }

            if (!string.IsNullOrEmpty(endTime))
            {
                var end = DateTime.ParseExact(endTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                lOrder = lOrder.Where(x => x.OrderTime <= end).ToList();
            }

            _tmpOrders?.Clear();
            _tmpOrders?.AddRange(lOrder);

            ViewData["MealTime"] = mealTime;
            ViewBag.StartTime = startTime; // "2021-12-21"
            ViewBag.EndTime = endTime;

            return View(lOrder);
        }

        public async Task<IActionResult> Export()
        {
            var result = await _orderService.ExportCsvWithList(_tmpOrders);
            var path = Path.Combine(this._pWwwRoot, "export");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filename = DateTime.Now.ToString("Export_HHmmss_ddMMyyyy") + ".csv";
            System.IO.File.WriteAllText(Path.Combine(path, filename), result);  

            return RedirectToAction("Index");
        }
    }
}
