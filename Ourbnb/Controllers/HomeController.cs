using Microsoft.AspNetCore.Mvc;
using Ourbnb.Models;
using System.Diagnostics;

namespace Ourbnb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       }
}