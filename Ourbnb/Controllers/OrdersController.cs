using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Ourbnb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace Ourbnb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Rental> _Rrepository;
        private readonly IRepository<Customer> _Crepository;


        public OrdersController(ILogger<OrdersController> logger, IRepository<Order> repository, IRepository<Customer> crepository, IRepository<Rental> Rrepository)
        {
            _logger = logger;
            _repository = repository;
            _Crepository = crepository;
            _Rrepository = Rrepository;
        }
        public async Task<CreateOrder> ViewModel()
        {
            var customers = await _Crepository.GetAll();
            var rentals = await _Rrepository.GetAll();


            var CreateOrder = new CreateOrder
            {

                CustomerList = customers.Select(customer => new SelectListItem
                {

                    Value = customer.CustomerId.ToString(),
                    Text = customer.CustomerId.ToString() + " : " + customer.FirstName + " " + customer.LastName
                }).ToList(),

                Order = new Order(),
                RentalList = rentals.Select(rental => new SelectListItem
                {
                    Value = rental.RentalId.ToString(),
                    Text = rental.RentalId.ToString() + " : " + rental.Name
                }).ToList(),
            };

            return CreateOrder;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListofOrders()
        {
            var orders = await _repository.GetAll();
            return View(orders);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var CreateOrder = await ViewModel();
            return View(CreateOrder);
        }
        [HttpPost]

        public async Task<IActionResult> Create(Order order)
        {
            var CreateOrder = await ViewModel();
            CreateOrder.Order = order;
            try
            {
                var customer = await _Crepository.getObjectById(order.CustomerId);
                var rental = await _Rrepository.getObjectById(order.RentalId);

                if (customer == null || rental == null)
                {
                    return View(CreateOrder);
                }
                var Days = order.To - order.From;
                var total = Days.Days * rental.Price;

                Order newOrder = new Order
                {
                    Customer = customer,
                    Rental = rental,
                    CustomerId = order.CustomerId,
                    RentalId = order.RentalId,
                    From = order.From,
                    To = order.To,
                    TotalPrice = total,
                    Rating = order.Rating,
                };

                bool ok = await _repository.Create(newOrder);
                if (!ok)
                {
                    return View(CreateOrder);
                }
                rental.UpdateRating();
                await _Rrepository.Update(rental);
                return RedirectToAction(nameof(ListofOrders));
            }
            catch (Exception ex)
            {
                return View(CreateOrder);
            }
        }
    }

}

