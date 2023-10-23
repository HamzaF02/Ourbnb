using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Ourbnb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRepository<Rental> _repository;
        private readonly IRepository<Customer> _Crepository;
        private readonly ILogger<RentalController> _logger;

        public async Task<CreateRental> ViewModel()
        {
            var owners = await _Crepository.GetAll();
            var CreateRental = new CreateRental
            {

                OwnersList = owners.Select(owner => new SelectListItem
                {

                    Value = owner.CustomerId.ToString(),
                    Text = owner.CustomerId.ToString() + " : " + owner.FirstName + " " + owner.LastName
                }).ToList(),

                Rental = new Rental()
            };

            return CreateRental;
        }

        public RentalController(IRepository<Rental> rentalRepository, IRepository<Customer> Crepository, ILogger<RentalController> logger)
        {
            _repository = rentalRepository;
            _Crepository = Crepository;
            _logger = logger;
        }

        public async Task<IActionResult> Table()
        {
            _logger.LogInformation("This is an error message");
            _logger.LogWarning("This is a warning");
            _logger.LogError("Error!!!!");
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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var CreateRental = await ViewModel();
            return View(CreateRental);
        }
        [HttpPost]
     
        public async Task<IActionResult> Create(Rental rental)
        {
            var CreateRental = await ViewModel();
            CreateRental.Rental = rental;
            try { 
                var owner = await _Crepository.getObjectById(rental.OwnerId);
                if(owner == null)
                {
                    return View(CreateRental);
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
                    return View(CreateRental);
                }
                return RedirectToAction(nameof(Grid));
            }catch (Exception ex)
            {
                return View(CreateRental);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var rental = await _repository.getObjectById(id);
            if (rental == null)
            {
                return NotFound("Nothing here");
            }
            var CreateRental = await ViewModel();
            CreateRental.Rental = rental;

            return View(CreateRental);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Rental rental)
        {
            var CreateRental = await ViewModel();
            CreateRental.Rental = rental;
            try
            {
                bool ok = await _repository.Update(rental);
                if (!ok)
                {
                    return View(CreateRental);
                }
                return RedirectToAction(nameof(Grid));
            }
            catch (Exception ex)
            {
                return View(CreateRental);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = await _repository.getObjectById(id);
            if (rental == null)
            {

                return BadRequest("Something went wrong, return to home page");
            }
            return View(rental);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool OK = await _repository.Delete(id);
            if (OK) { return RedirectToAction(nameof(Grid)); }


            return BadRequest("Rental deletion failed, return to homepage");
        }


    }
}
            
