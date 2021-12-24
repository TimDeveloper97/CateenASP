using API;
using Microsoft.AspNetCore.Mvc;

namespace CanteenASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        readonly OrderService _orderService;
        string _pWwwRoot;

        public OrderController(IHostEnvironment webHostEnviroment)
        {
            this._orderService = new OrderService();
            _pWwwRoot = Path.Combine(webHostEnviroment.ContentRootPath, "wwwroot");
        }

        public async Task<IActionResult> Index(string mealTime, string startTime, string endTime)
        
        {
            
            var lOrder = await _orderService.GetAll();
            ViewData["MealTime"] = mealTime;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            return View(lOrder);
        }

        public async Task<IActionResult> Export()
        {
            var result = await _orderService.ExportCsv();
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
