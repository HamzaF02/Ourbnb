using Microsoft.AspNetCore.Mvc;
using Ourbnb.Models;
using System.Diagnostics;

namespace Ourbnb.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        public IActionResult regPage()
        {
            return View();
        }

       }

}