using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRepository<Rental> _repository;
        private readonly IRepository<Customer> _Crepository;
        private readonly ILogger<RentalController> _logger;

        public RentalController(IRepository<Rental> rentalRepository, IRepository<Customer> Crepository, ILogger<RentalController> logger)
        {
            _repository = rentalRepository;
            _Crepository = Crepository;
            _logger = logger;
        }

        public async Task<IActionResult> Table()
        {
            var rentals =  await _repository.GetAll();
            ViewBag.CurrentViewName = "Table";
            return View(rentals);
        }

        public async Task<IActionResult> Grid()
        {
            ViewBag.CurrentViewName = "Grid";
            var rentals = await _repository.GetAll();
            return View(rentals);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rental = await _repository.getObjectById(id);
            if(rental == null)
            {
                return NotFound("Nothing here");
            }
        
            return View(rental);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Rental rental)
        {
            try { 
                Customer owner = await _Crepository.getObjectById(rental.OwnerId);
                if(owner == null)
                {
                    return BadRequest("Owner not Found");
                }

                Rental newRental = new Rental
                {
                    Name = rental.Name,
                    Description = rental.Description,
                    FromDate = rental.FromDate,
                    ToDate = rental.ToDate,
                    Owner = owner,
                    OwnerId = rental.OwnerId,
                    Price = rental.Price,
                    Bilder = rental.Bilder,
                    Location = rental.Location,
                    Rating = 0
                };

                bool ok = await _repository.Create(newRental);
                if(!ok) {
                    return View(rental);
                }
                return RedirectToAction(nameof(Grid));
            }catch (Exception ex)
            {
                return View(rental);
            }
            
           
        }
    }
}
