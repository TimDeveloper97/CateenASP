using API;
using Microsoft.AspNetCore.Mvc;

namespace CanteenASP.Controllers
{
    public class UserController : Controller
    {
        readonly UserService _userService;
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
            if (res.Success)
            {
                HttpContext.Session.SetString("UserId", res.Result.Id);

                if (res.Result.Description == "Admin")
                {
                    HttpContext.Session.SetString("Role", "Admin");
                    return RedirectToAction("Index", "Food", new { area = "Admin" });
                }
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
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Forgot()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Forgot(Model.User user)
        {
            var res = await _userService.ResetPassword(user.UserName, user.Password, user.Phone);
            if (res.Success)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
    }
}
