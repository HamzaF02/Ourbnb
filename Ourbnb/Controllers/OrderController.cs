using Microsoft.AspNetCore.Mvc;
using Ourbnb.Models;

namespace Ourbnb.Controllers;

    public class OrderController : Controller
    {
        public IActionResult Table()
        {
            List<Order> Orders = new List<Order>(); 
            return View(Orders);
        }
    }

