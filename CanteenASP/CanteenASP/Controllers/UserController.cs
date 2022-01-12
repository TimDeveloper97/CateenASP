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
                
                if (res.Result.Description.ToLower() == "admin")
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
            var res = await _userService.ForgotPassword(user.UserName, user.Password, user.Phone);
            if (res.Success)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reset()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Reset(string oldPassword, string password)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if(string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index");
            }
            var res = await _userService.ResetPassword(userId, oldPassword, password);
            if (!res.Success)
            {
               
                ViewData["Message"] = res.Message;
                return View();
            }
            ViewData["Message"] = res.Message;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Infor()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index");
            }
            var user = await _userService.GetItem(userId);
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeInfor()
        {

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index");
            }
            var user = await _userService.GetItem(userId);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeInfor(Model.User user)
        {
            user.DisplayName = user.LastName + " " + user.FirstName;
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index");
            }
            user.Id = userId;
            var res = await _userService.ChangeInfor(user);
            if (res)
            {
                return RedirectToAction("Infor");
            }
            return View(user);
        }
    }
}
