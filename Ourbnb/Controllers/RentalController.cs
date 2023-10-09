using Microsoft.AspNetCore.Mvc;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        public IActionResult rentalspage()
        {
            return View();
        }
    }
}
