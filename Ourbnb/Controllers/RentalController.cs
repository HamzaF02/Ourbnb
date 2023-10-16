using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using System.Drawing.Printing;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
      
     
        public IActionResult Table()
        {
            var rentals = GetRentals();
            ViewBag.CurrentViewName = "Table";
            return View(rentals);
        }

        public IActionResult Grid()
        {
            var rentals = GetRentals();
            ViewBag.CurrentViewName = "Grid";
            return View(rentals);
        }

        public List<Rental> GetRentals()
        {
            var rentals = new List<Rental>();
            var owner1 = new Customer
            {
                CustomerId = 1,
                FirstName = "A",
                LastName = "B",
                Address = "C",
                Phone = 122,
                Email = "Gmail"
            };


            var rental1 = new Rental
            {
                RentalId = 1,
                Name = "House by the waterfalls",
                Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                FromDate = new DateOnly(2015, 10, 10),
                ToDate = new DateOnly(2015, 10, 20),
                Rating = 3.5,
                Location = "Gran",
                Price = 150,
                Bilder = {},
                OwnerId = 1,
                Owner = owner1
            };

            var rental2 = new Rental
            {
                RentalId = 2,
                Name = "House by the waterfalls",
                Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                FromDate = new DateOnly(2015, 10, 10),
                ToDate = new DateOnly(2015, 10, 10),
                Rating = 3.5,
                Location = "Gran",
                Price = 150,
                Bilder = {},
                OwnerId = 1,
                Owner = owner1
            };

            var rental3 = new Rental
            {
                RentalId = 3,
                Name = "House by the waterfalls",
                Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                FromDate = new DateOnly(2015, 10, 10),
                ToDate = new DateOnly(2015, 10, 20),
                Rating = 3.5,
                Location = "Gran",
                Price = 150,
                Bilder = { },
                OwnerId = 1,
                Owner = owner1
            };

            var rental4 = new Rental
            {
                RentalId = 4,
                Name = "House by the waterfalls",
                Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                FromDate = new DateOnly(2015, 10, 10),
                ToDate = new DateOnly(2015, 10, 10),
                Rating = 3.5,
                Location = "Gran",
                Price = 150,
                Bilder = { },
                OwnerId = 1,
                Owner = owner1
            };

            rentals.Add(rental1);
            rentals.Add(rental2);
            rentals.Add(rental3);
            rentals.Add(rental4);
            return rentals;
        }
    }
}
