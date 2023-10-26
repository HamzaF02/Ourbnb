using Microsoft.AspNetCore.Mvc;
using Ourbnb.DAL;
using Ourbnb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Ourbnb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Castle.Core.Resource;

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
        public async Task<CreateOrder?> ViewModel(int id)
        {
            var customers = await _Crepository.GetAll();
            var rental = await _Rrepository.getObjectById(id);
            if (rental == null) { return null; }


            var CreateOrder = new CreateOrder
            {

                CustomerList = customers.Select(customer => new SelectListItem
                {

                    Value = customer.CustomerId.ToString(),
                    Text = customer.CustomerId.ToString() + " : " + customer.FirstName + " " + customer.LastName
                }).ToList(),

                Order = new Order(),
                Rental = rental
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
        public async Task<IActionResult> Create(int id)
        {
            var CreateOrder = await ViewModel(id);
            return View(CreateOrder);
        }
        [HttpPost]

        public async Task<IActionResult> Create(Order order)
        {
            var CreateOrder = await ViewModel(order.RentalId);
            if (CreateOrder == null) { return BadRequest("Something went wrong, return home"); }
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
                await UpdateRental(rental);
                return RedirectToAction(nameof(ListofOrders));
            }
            catch (Exception ex)
            {
                return View(CreateOrder);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var order = await _repository.getObjectById(id);
            if (order == null)
            {
                return NotFound("Something went wrong, go to homepage");
            }
            var CreateOrder = await ViewModel(order.RentalId);
            CreateOrder.Order = order;
            return View(CreateOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Order order)
        {
            var CreateOrder = await ViewModel(order.RentalId);
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
                    Rental = order.Rental,
                    CustomerId = order.CustomerId,
                    RentalId = order.RentalId,
                    From = order.From,
                    To = order.To,
                    TotalPrice = total,
                    Rating = order.Rating,
                };
                bool ok = await _repository.Update(newOrder);
                if (ok)
                {
                    await UpdateRental(order.RentalId);
                    return RedirectToAction(nameof(ListofOrders));
                }
                return View(CreateOrder);
            }
            catch (Exception ex)
            {
                return View(CreateOrder);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _repository.getObjectById(id);
            if (order == null)
            {
                return BadRequest("Something went wrong, return to home page");
            }
            return View(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _repository.getObjectById(id);
            bool OK = await _repository.Delete(id);
            if (OK)
            {
                await UpdateRental(order.RentalId);
                return RedirectToAction(nameof(ListofOrders));
            }
            return BadRequest("Rental deletion failed, return to homepage");
        }

        public async Task UpdateRental(int id)
        {
            var rental = await _Rrepository.getObjectById(id);
            rental.UpdateRating();
            await _Rrepository.Update(rental);
        }
        public async Task UpdateRental(Rental rental)
        {
            rental.UpdateRating();
            await _Rrepository.Update(rental);
        }
    }

}

