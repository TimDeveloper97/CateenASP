using Microsoft.AspNetCore.Mvc;
using Model;

namespace CanteenASP.Controllers.Components
{
    public class MealsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Order> listOrder)
        {
            return View("Default", listOrder);
        }
    }
}
