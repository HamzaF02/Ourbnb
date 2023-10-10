using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        private readonly RentalDbContext _rentalDbContext;
        public IActionResult Rentalspage()
        {
            return View();
        }
    }
}
