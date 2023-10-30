using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ourbnb.Models;

namespace Ourbnb.ViewModels
{
    public class CreateRental
    {
        public Rental Rental { get; set; } = default!;
        public Customer Owner { get; set; } = default!;
    }
}
