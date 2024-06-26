﻿using Microsoft.AspNetCore.Mvc;
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
            //Gets all rentals
            var rentals =  await _repository.GetAll();

            //Checks if there was a problem
            if (rentals == null)
            {
                _logger.LogError("[RentalController] Rental list not found while executing _repository.GetAll()");
                return NotFound("Rental list not found");
            }
            //Returns view
            return View(rentals);
        }

        //Returns Grid view with all Orders
        public async Task<IActionResult> Grid()
        {
            //Gets all rentals
            var rentals = await _repository.GetAll();

            //Checks if there was a problem
            if (rentals == null)
            {
                _logger.LogError("[RentalController] Rental list not found while executing _repository.GetAll()");
                return NotFound("Rental list not found");
            }
            
            //Returns view
            return View(rentals);
        }

        public async Task<IActionResult> Details(int id)
        {
            //Gets current Rental
            var rental = await _repository.getObjectById(id);

            //Checks if there was a problem
            if (rental == null)
            {
                _logger.LogError("[RentalController] Rental list not found for the RentalId {RentalId:0000}", id);
                return NotFound("Rental not found for the RentalId");
            }
            
            //Returns View
            return View(rental);
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            //Creates viewmodel and checks it
            var CreateRental = await ViewModel();
            if(CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Create()");
                return NotFound("ViewModel Error, return home");
            }
            //Return the view with ViewModel
            return View(CreateRental);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Rental rental)
        {
            //Creates viewmodel and checks it
            var CreateRental = await ViewModel();
            if (CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Create()");
                return NotFound("ViewModel Error, return home");
            }

            //Adds current rental to viewmodel
            CreateRental.Rental = rental;

            //try catch for creation incase of exception
            try
            {
                //Gets Owner and checks it
                var owner = await _Crepository.getObjectById(rental.OwnerId);
                if(owner == null)
                {
                    return BadRequest("Owner not Found");
                }

                //Initilize rental
                Rental newRental = new Rental { };

                //Checks if dates are valid
                int checkDate = DateTime.Compare(rental.FromDate, rental.ToDate);
                if (checkDate < 0 && rental.FromDate >= DateTime.Now.Date)
                {
                    //Creation of Rental object
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
                    //logs and return error message to view
                    _logger.LogWarning("Dates are not valid");
                    CreateRental.message = "Dates are not valid";
                    return View(CreateRental);
                }

                //Creates Rental and checks for mistakes

                bool ok = await _repository.Create(newRental);
                if(!ok) {
                    //logs and return error message to view
                    _logger.LogWarning("[RentalController] Rental creation failed {@rental}", rental);
                    CreateRental.message = "Creation failed";
                    return View(CreateRental);
                }
                //Redirects to Main Rentals Page
                return RedirectToAction(nameof(Grid));
            }catch (Exception ex)
            {
                //In case of exception it logs error and goes back to input field with message
                _logger.LogWarning("[RentalController] Rental creation failed {@rental}, error message: {ex}", rental, ex.Message);
                CreateRental.message = "Creation failed";
                return View(CreateRental);
            }
        }

        //Gets page for Update
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            //Finds and checks Rental for updating
            var rental = await _repository.getObjectById(id);
            if (rental == null)
            {
                _logger.LogError("[RentalController] Rental not found when updating the RentalId {RentalId:0000}", id);
                return BadRequest("Rental not found for the RentalId");
            }

            //Creates viewmodel and checks it
            var CreateRental = await ViewModel();
            if (CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Update()");
                return NotFound("ViewModel Error, return home");
            }
            //Sets otherwise default rental to found rental and returns view model
            CreateRental.Rental = rental;

            return View(CreateRental);
        }

        //Update with input
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(Rental rental)
        {
            //Creates ViewModel incase something goes wrong and checks it
            var CreateRental = await ViewModel();
            if (CreateRental == null)
            {
                _logger.LogError("[RentalController] Error making ViewModel while executing Update()");
                return NotFound("ViewModel Error, return home");
            }

            //Adds current rental to viewmodel
            CreateRental.Rental = rental;

            //try catch for creation incase of exception
            try
            {
                //Gets owner and checks it
                var owner = await _Crepository.getObjectById(rental.OwnerId);
                if (owner == null)
                {
                    return BadRequest("Owner does not exist!");
                }

                //Initiliaze rental
                Rental newRental = new Rental { };

                //Checks Date 
                int checkDate = DateTime.Compare(rental.FromDate, rental.ToDate);
                if (checkDate < 0 && rental.FromDate >= DateTime.Now.Date)
                {
                    //Creates rental and adds RentalId to update
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
                    //Logs error and returns view with message
                    _logger.LogWarning("Dates are not valid");
                    CreateRental.message = "Dates are not valid";
                    return View(CreateRental);
                }
                //Repository updates and checks it
                bool ok = await _repository.Update(rental);
                if (!ok)
                {
                    //Logs error and returns view with message
                    _logger.LogWarning("[RentalController] Rental update failed {@rental}", rental);
                    CreateRental.message = "Rental Update Failed";
                    return View(CreateRental);
                }
                return RedirectToAction(nameof(Grid));
            }
            catch (Exception ex)
            {
                //In case of exception it logs error and returns view with message
                _logger.LogWarning("[RentalController] Rental update failed {@rental}, error message: {ex}", rental,ex.Message);
                CreateRental.message = "Rental update failed";
                return View(CreateRental);
            }
        }

        //Get page of deletion
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //Finds and checks rental to delete, if not logs and returns badrequest

            var rental = await _repository.getObjectById(id);
            if (rental == null)
            {
                _logger.LogError("[RentalController] Rental not found for the RentalId {RentalId:0000}", id);
                return BadRequest("Something went wrong, return to home page");
            }
            //Returns view
            return View(rental);
        }

        //Deletes Rental
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Delets Rental and if it fails, deals with it accordingly with logs and BadRequest
            bool OK = await _repository.Delete(id);
            if (!OK) {
                _logger.LogError("[RentalController] Rental deletion failed for the RentalId {RentalId:0000}", id);
                return BadRequest("Rental deletion failed, return to homepage");
            }

            //Redirects to Rentals Main page
            return RedirectToAction(nameof(Grid));
            
        }


    }
}
            
