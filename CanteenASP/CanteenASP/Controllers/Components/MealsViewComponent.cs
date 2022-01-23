using Microsoft.AspNetCore.Mvc;
using Model;
using System.Globalization;

namespace CanteenASP.Controllers.Components
{
    public class MealsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Order listOrder)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            if(listOrder != null)
            {
                listOrder.TotalPrice = double.Parse(listOrder.TotalPrice).ToString("#,###", cul.NumberFormat);

            }
            return View("Default", listOrder);
        }
    }
}
