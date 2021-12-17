using API;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace CanteenASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        readonly UserService _userService;

        public UserController()
        {
            this._userService = new UserService();
        }

        public async Task<IActionResult> Index()
        {
            var lUser = await _userService.GetAll();
            return View(lUser);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            user.DisplayName = user.FirstName + " " + user.LastName;
            user.Password = Common.MD5Hash(input: user.Password);

            var result = await _userService.Create(user);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var food = await _userService.Read(id);
            return View(food);
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Edit(User user)
        {
            user.DisplayName = user.FirstName + " " + user.LastName;
            var result = await _userService.Update(user);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
