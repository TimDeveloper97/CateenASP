using Microsoft.AspNetCore.Mvc;
using Model;
using System.Globalization;

namespace CanteenASP.Controllers.Components
{
    public class MealsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Order> listOrder)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            if(listOrder != null)
            {
                foreach (var item in listOrder)
                {
                    item.TotalPrice = double.Parse(item.TotalPrice).ToString("#,###", cul.NumberFormat);
                }
            }
            return View("Default", listOrder);
        }
    }
}
