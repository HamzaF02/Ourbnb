using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using Ourbnb.DAL;
using Ourbnb.Models;
using Ourbnb.ViewModels;
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

        public async Task<CreateCustomer> ViewModel()
        {
            var owners = await _repository.GetAll();
            var CreateCustomer = new CreateCustomer
            {


                customer = new Customer()
            };

            return CreateCustomer;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var createCustomer = await ViewModel();
            return View(createCustomer);
        }
        [HttpPost]
        public async Task<IActionResult> Register(Customer customer)
        {

            var CreateCustomer = await ViewModel();
            CreateCustomer.customer = customer;

            if (ModelState.IsValid)
            {
                try
                {

                    Customer newCustomer = new Customer
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Address = customer.Address,
                        Phone = customer.Phone,
                        Email = customer.Email,
                        IdentityId = customer.IdentityId
                        

                    };


                    bool ok = await _repository.Create(newCustomer);
                    if (!ok)
                    {
                        return View(CreateCustomer);
                    }
                    return RedirectToAction(nameof(Register));
                }
                catch (Exception ex)
                {
                    return View(CreateCustomer);
                }
            }
            else
            {
                return View(CreateCustomer);
            }
        }

    }

}
