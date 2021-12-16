using API;
using Microsoft.AspNetCore.Mvc;

namespace CanteenASP.Controllers
{
    public class UserController : Controller
    {
        UserService _userService;
        public UserController()
        {
            _userService = new UserService();   
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var res = await _userService.Login(username, password);
            if(res.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = res.Message;
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Model.User user)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.Register(user);
                if (res)
                {
                    return RedirectToAction("Index");
                }
                
            }
            return View(user);
        }
    }
}
