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
using System.Security.Claims;

namespace Ourbnb.Controllers
{
    public class OrdersController : Controller
    {
        //Serilogger
        private readonly ILogger<OrdersController> _logger;

        //Repositories for Order, Rental and Customer
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Rental> _Rrepository;
        private readonly IRepository<Customer> _Crepository;

        //Constructor for class and defines variables
        public OrdersController(ILogger<OrdersController> logger, IRepository<Order> repository, IRepository<Customer> crepository, IRepository<Rental> Rrepository)
        {
            _logger = logger;
            _repository = repository;
            _Crepository = crepository;
            _Rrepository = Rrepository;
        }
        //Creates CreateOrder ViewModel
        public async Task<CreateOrder?> ViewModel(int id)
        {
            //Gets the Identity of current logged inn user, all customers and a rental using parameter
            var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customers = await _Crepository.GetAll();
            var rental = await _Rrepository.getObjectById(id);

            //Checks for null values
            if (rental == null || customers == null || identity == null)
            {
                _logger.LogError("[OrdersController] rental or customer list not found while executing _Rrepository.GetObjectById(id)");
                return null;
            }

            //Finds Customer that matches to Identity
            Customer customer = new Customer();

            foreach (var i in customers)
            {
                if (i.IdentityId == identity) { 
                    customer = i;
                    break;
                }
            }

            //checks if it was not found and logs incase of true
            if (customer == null)
            {
                _logger.LogError("[OrdersController] Customer matching identityId in list not found while executing _Rrepository.GetObjectById(id)");
                return null;
            }
                        
            //Creates the view Model and returns it
            var CreateOrder = new CreateOrder
            {
                Order = new Order(),
                Rental = rental,
                Customer = customer,
            };

            return CreateOrder;
        }

        //Returns view with all Orders
        [Authorize]
        public async Task<IActionResult> ListofOrders()
        {
            //Gets all orders
            var orders = await _repository.GetAll();

            //Checks if there was a problem
            if (orders == null)
            {
                _logger.LogError("[OrderController] Orderlist not found while executing _repository-GetAll()");
                return NotFound("Orderlist not found");
            }

            //Returns view
            return View(orders);
        }


        //Gets the page for Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            //Creates viewmodel and checks it
            var CreateOrder = await ViewModel(id);
            if (CreateOrder == null)
            {
                _logger.LogError("[OrderController] Error making ViewModel while executing Create()");
                return NotFound("ViewModel Error, return home");
            }
            //Return the view with ViewModel
            return View(CreateOrder);
        }

        //Input from client to Create order
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Order order)
        {
            //Creates viewModel incase something goes wrong and checks it
            var CreateOrder = await ViewModel(order.RentalId);
            if (CreateOrder == null)
            {
                _logger.LogError("[OrderController] Error making ViewModel while executing Create()");
                return NotFound("ViewModel Error, return home");
            }
            //Creates viewmodel and checks it
            CreateOrder.Order = order;

            //try catch for creation incase of exception
            try
            {
                //Gets customer and rental assosiated with order
                var customer = await _Crepository.getObjectById(order.CustomerId);
                var rental = await _Rrepository.getObjectById(order.RentalId);

                //Checks if values are valid
                if (customer == null || rental == null)
                {
                    _logger.LogError("[OrdersController] Failed to find customer or rental with _Crepository.getObjectById() or _Rrepository.getObjectById()");
                    return View(CreateOrder);
                }

                //Calculates total Price
                var Days = order.To - order.From;
                var totalPrice = Days.Days * rental.Price;

                //Checks Date and responds accordingly
                Order newOrder = new Order { };
                if(order.From >= rental.FromDate && order.From >= DateTime.Now.Date && order.From < rental.ToDate && order.From < order.To && order.To <= rental.ToDate)
                {
                    //Creation of Order
                    newOrder = new Order
                    {
                        Customer = customer,
                        Rental = rental,
                        CustomerId = order.CustomerId,
                        RentalId = order.RentalId,
                        From = order.From,
                        To = order.To,
                        TotalPrice = totalPrice,
                        Rating = order.Rating,
                    };
                }
                else
                {
                    _logger.LogError("dates for order are invalid");
                    return View(CreateOrder);
                }

                
                //Creates Order and checks for mistakes
                bool ok = await _repository.Create(newOrder);
                if (!ok)
                {
                    _logger.LogWarning("[OrdersController] newOrder creation failed {@newOrder}", newOrder);
                    return View(CreateOrder);
                }
                
                //Updates rentals Rating
                await UpdateRental(rental);

                //Redirects to Main Orders Page
                return RedirectToAction(nameof(ListofOrders));
            }
            catch (Exception ex)
            {
                //In case of exception it logs error and goes back to input field
                _logger.LogWarning("[OrdersController] newOrder creation failed {@CreateOrder}, error message: {ex}", CreateOrder, ex.Message);
                return View(CreateOrder);
            }
        }

        //Get page of Update
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            //Finds and checks order for updating
            var order = await _repository.getObjectById(id);
            if (order == null)
            {
                _logger.LogError("[OrdersController] Order not found for the id {@id}", id);
                return NotFound("Something went wrong, go to homepage");
            }

            //Creates viewmodel and checks it
            var CreateOrder = await ViewModel(order.RentalId);
            if (CreateOrder == null)
            {
                _logger.LogError("[OrderController] Error making ViewModel while executing Update()");
                return NotFound("ViewModel Error, return home");
            }
            //Sets otherwise default order to found order
            CreateOrder.Order = order;

            //Return the view with ViewModel
            return View(CreateOrder);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(Order order)
        {
            //Creates viewModel incase something goes wrong and checks it
            var CreateOrder = await ViewModel(order.RentalId);
            if (CreateOrder == null)
            {
                _logger.LogError("[OrderController] Error making ViewModel while executing Update()");
                return NotFound("ViewModel Error, return home");
            }

            //Creates viewmodel and checks it
            CreateOrder.Order = order;

            //try catch for creation incase of exception
            try
            {
                //Gets customer and rental assosiated with order
                var customer = await _Crepository.getObjectById(order.CustomerId);
                var rental = await _Rrepository.getObjectById(order.RentalId);

                //Checks if values are valid
                if (customer == null || rental == null)
                {
                    _logger.LogError("[OrdersController] Failed to find customer or rental with _Crepository.getObjectById() or _Rrepository.getObjectById()");
                    return View(CreateOrder);
                }

                //Calculates total Price
                var Days = order.To - order.From;
                var total = Days.Days * rental.Price;

                //Checks Date and responds accordingly
                Order newOrder = new Order { };
                if (order.From >= rental.FromDate && order.From >= DateTime.Now.Date && order.From < rental.ToDate && order.From < order.To && order.To <= rental.ToDate)
                {
                    //Creation of Order, OrderId is added since we are updating a value
                    newOrder = new Order
                    {
                        OrderId = order.OrderId,
                        Customer = customer,
                        Rental = rental,
                        CustomerId = order.CustomerId,
                        RentalId = order.RentalId,
                        From = order.From,
                        To = order.To,
                        TotalPrice = total,
                        Rating = order.Rating,
                    };
                }
                else
                {
                    _logger.LogError("dates for order are invalid");
                    return View(CreateOrder);
                }
                //Repository updates and checks it
                bool ok = await _repository.Update(newOrder);
                if (ok)
                {
                    //Updates rental rating and goes back to listoforders
                    await UpdateRental(order.RentalId);
                    return RedirectToAction(nameof(ListofOrders));
                }
                _logger.LogError("[OrdersController] Order failed to update {@order}", order);
                return View(CreateOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError("[OrdersController] Order failed to update {@order}, error message: {ex} ", order, ex.Message);
                return View(CreateOrder);
            }
            //Logging incase of errors
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            //Finds and checks order to delete, if not logs and returns notfound
            var order = await _repository.getObjectById(id);
            if (order == null)
            {
                _logger.LogError("[OrdersController] Order not found for the id {@id}", id);
                return NotFound("Something went wrong, return to home page");
            }
            return View(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Finds order and deletes it.
            var order = await _repository.getObjectById(id);
            bool OK = await _repository.Delete(id);

            //If all goes right it deletes order and updates rental Rating
            if (OK)
            {
                if(order != null)
                {
                    await UpdateRental(order.RentalId);

                }
                return RedirectToAction(nameof(ListofOrders));
            }

            //Error handling incase of deletion mistake
            _logger.LogError("[OrdersController] Order deletion failed for the order.Rentalid {@order.RentalId}", id);
            return BadRequest("Rental deletion failed, return to homepage");
        }

        public async Task UpdateRental(int id)
        {
            //Finds rental from id
            var rental = await _Rrepository.getObjectById(id);
            if (rental != null)
            {
                //If rental is found update its rating through its own function
                rental.UpdateRating();
                //Update in database
                await _Rrepository.Update(rental);
                
            }
        }
        public async Task UpdateRental(Rental rental)
        {
            //Update its rating through its own function
            rental.UpdateRating();
            //Update in database
            await _Rrepository.Update(rental);
        }
    }

}


