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
        public RentalController(IRepository<Rental> rentalRepository)
        {
            _repository = rentalRepository;
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
            if (ModelState.IsValid)
            {
                bool OK = await _repository.Create(rental);
                if (OK)
                {
                    return RedirectToAction(nameof(Grid));
                }
            }
            return View(rental);
        }
    }
}
