using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using Microsoft.EntityFrameworkCore;
namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        private readonly RentalDbContext _rentalDbContext;
        public RentalController(RentalDbContext rentalDbContext)
        {
            _rentalDbContext = rentalDbContext;
        }

        public async Task<IActionResult> Table()
        {
            var rentals = await _rentalDbContext.Rentals.ToListAsync();
            ViewBag.CurrentViewName = "Table";
            return View(rentals);
        }

        public async Task<IActionResult> Grid()
        {
            var rentals = await _rentalDbContext.Rentals.ToListAsync();
            ViewBag.CurrentViewName = "Grid";
            return View(rentals);
        }
    }
}
