using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using System.Drawing.Printing;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        private readonly RentalDbContext _rentalDbContext;

        public RentalController(RentalDbContext rentalDbContext)
        {
            _rentalDbContext = rentalDbContext;
        }
        public IActionResult Rentalspage()
        {
            List<Rental> rentals = _rentalDbContext.Rentals.ToList();
            return View(rentals);
        }
    }
}
