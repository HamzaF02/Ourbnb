using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Ourbnb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;
using Castle.Core.Resource;
using System.Security.Principal;
using System.Security.Claims;

namespace Ourbnb.Controllers
{
    public class RentalController : Controller
    {
        //Repositories for Order, Rental and Customer
        private readonly IRepository<Rental> _repository;
        private readonly IRepository<Customer> _Crepository;

        //Serilogger
        private readonly ILogger<RentalController> _logger;

        //Constructor for class and defines variables
        public RentalController(IRepository<Rental> rentalRepository, IRepository<Customer> Crepository, ILogger<RentalController> logger)
        {
            _repository = rentalRepository;
            _Crepository = Crepository;
            _logger = logger;
        }

        //Creates CreateRental ViewModel
        public async Task<CreateRental?> ViewModel()
        {
            //Gets the Identity of current logged inn user and all possible owners
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var owners = await _Crepository.GetAll();

            //Checks for null values
            if (owners == null || identity == null)
            {
                _logger.LogError("[RentalController] Owners list or userId not found while executing _repository.GetObjectById(id)");
                return null;
            }

            //Finds owner that matches to Identity
            Customer owner = new Customer();
            foreach (var i in owners)
            {
                if (i.IdentityId == identity)
                {
                    owner = i;
                    break;
                }
            }

            //Checks if it was not found and logs incase of true
            if (owner == default(Customer))
            {
                _logger.LogError("[RentalController] Owner matching identityId in list not found while executing _repository.GetObjectById(id)");
                return null;
            }

            //Creates ViewModel
            var CreateRental = new CreateRental
            {
                Rental = new Rental(),
                Owner = owner
            };

            //Returns ViewModel
            return CreateRental;
        }

        //Returns Table view with all Orders
        public async Task<IActionResult> Table()
        {
            var rentals =  await _repository.GetAll();
            if(rentals == null)
            {
                _logger.LogError("[RentalController] Rental list not found while executing _repository.GetAll()");
                return NotFound("Rental list not found");
            }
            ViewBag.CurrentViewName = "Table";
            return View(rentals);
        }

        //Returns Grid view with all Orders

        public async Task<IActionResult> Grid()
        {
            var rentals = await _repository.GetAll();
            if(rentals == null)
            {
                _logger.LogError("[RentalController] Rental list not found while executing _repository.GetAll()");
                return NotFound("Rental list not found");
            }
            ViewBag.CurrentViewName = "Grid";
            return View(rentals);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rental = await _repository.getObjectById(id);
            if(rental == null)
            {
                _logger.LogError("[RentalController] Rental list not found for the RentalId {RentalId:0000}", id);
                return NotFound("Rental not found for the RentalId");
            }
        
            return View(rental);
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var CreateRental = await ViewModel();
            if(CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Create()");
                return NotFound("ViewModel Error, return home");
            }
            return View(CreateRental);
        }
        [HttpPost]
     
        public async Task<IActionResult> Create(Rental rental)
        {
            var CreateRental = await ViewModel();
            if (CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Create()");
                return NotFound("ViewModel Error, return home");
            }
            CreateRental.Rental = rental;
            try { 
                var owner = await _Crepository.getObjectById(rental.OwnerId);
                if(owner == null)
                {
                    return BadRequest("Owner not Found");
                }

                int checkDate = DateTime.Compare(rental.FromDate, rental.ToDate);

                Rental newRental = new Rental { };

                if (checkDate < 0 && rental.FromDate >= DateTime.Now.Date)
                {
                    newRental = new Rental
                    {
                        Name = rental.Name,
                        Description = rental.Description,
                        FromDate = rental.FromDate,
                        ToDate = rental.ToDate,
                        Owner = owner,
                        OwnerId = rental.OwnerId,
                        Price = rental.Price,
                        Image = rental.Image,
                        Location = rental.Location,
                        IdentityId = rental.IdentityId,
                        Rating = 0
                    };
                }
                else
                {
                    _logger.LogWarning("Dates are not valid");
                    CreateRental.message = "Dates are not valid";
                    return View(CreateRental);
                }
                


                bool ok = await _repository.Create(newRental);
                if(!ok) {
                    _logger.LogWarning("[RentalController] Rental creation failed {@rental}", rental);
                    CreateRental.message = "Creation failed";
                    return View(CreateRental);
                }
                return RedirectToAction(nameof(Grid));
            }catch (Exception ex)
            {
                _logger.LogWarning("[RentalController] Rental creation failed {@rental}, error message: {ex}", rental, ex.Message);
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
                _logger.LogError("[RentalController] Rental not found when updating the RentalId {RentalId:0000}", id);
                return BadRequest("Rental not found for the RentalId");
            }
            var CreateRental = await ViewModel();
            if (CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Update()");
                return NotFound("ViewModel Error, return home");
            }
            CreateRental.Rental = rental;

            return View(CreateRental);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Rental rental)
        {
            var CreateRental = await ViewModel();
            if (CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Update()");
                return NotFound("ViewModel Error, return home");
            }
            CreateRental.Rental = rental;
            try
            {
                var owner = await _Crepository.getObjectById(rental.OwnerId);
                if (owner == null)
                {
                    return BadRequest("Owner does not exist!");
                }
                int checkDate = DateTime.Compare(rental.FromDate, rental.ToDate);

                Rental newRental = new Rental { };

                if (checkDate < 0 && rental.FromDate >= DateTime.Now.Date)
                {
                    newRental = new Rental
                    {
                        RentalId = rental.RentalId,
                        Name = rental.Name,
                        Description = rental.Description,
                        FromDate = rental.FromDate,
                        ToDate = rental.ToDate,
                        Owner = owner,
                        OwnerId = rental.OwnerId,
                        Price = rental.Price,
                        Image = rental.Image,
                        Location = rental.Location,
                        IdentityId = rental.IdentityId,
                        Rating = 0
                    };
                }
                else
                {
                    _logger.LogWarning("Dates are not valid");
                    CreateRental.message = "Dates are not valid";
                    return View(CreateRental);
                }
                bool ok = await _repository.Update(rental);
                if (!ok)
                {
                    _logger.LogWarning("[RentalController] Rental update failed {@rental}", rental);
                    CreateRental.message = "Update Failed";
                    return View(CreateRental);
                }
                return RedirectToAction(nameof(Grid));
            }
            catch (Exception ex)
            {
                _logger.LogWarning("[RentalController] Rental update failed {@rental}, error message: {ex}", rental,ex.Message);
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
                _logger.LogError("[RentalController] Rental not found for the RentalId {RentalId:0000}", id);
                return BadRequest("Something went wrong, return to home page");
            }
            return View(rental);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool OK = await _repository.Delete(id);
            if (!OK) {
                _logger.LogError("[RentalController] Rental deletion failed for the RentalId {RentalId:0000}", id);
                return BadRequest("Rental deletion failed, return to homepage");

            }

            return RedirectToAction(nameof(Grid));
            
        }


    }
}
            
