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
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IRepository<Order> _repository;

        public OrdersController(ILogger<OrdersController> logger, IRepository<Order> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListofOrders()
        {
            var orders = await _repository.GetAll();
            return View(orders);
        }

    }

}

