using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using System.Diagnostics;

namespace Ourbnb.Controllers
{
    public class RegisterController : Controller

    {
        private readonly IRepository<Customer> _repository;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger, IRepository<Customer> customer)
        {
            _logger = logger;
            _repository = customer;
        }

        [HttpGet]
        public async Task<IActionResult> regPage()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
       

                Customer newCustomer = new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Phone= customer.Phone,
                    Email = customer.Email

                    
                };


                bool ok = await _repository.Create(newCustomer);
                if (!ok)
                {
                    return View();
                }
                return RedirectToAction();
            }
           
        }

    }

}