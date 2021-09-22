using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Info()
        {
            return View();
        }
    }
}
