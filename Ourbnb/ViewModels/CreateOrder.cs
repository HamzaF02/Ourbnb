using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ourbnb.Models;

namespace Ourbnb.ViewModels
{
    public class CreateOrder
    {
        public Order Order { get; set; } = default!;
        public Rental Rental { get; set; } = default!;
        public Customer Customer { get; set; } = default!;
    }
}
