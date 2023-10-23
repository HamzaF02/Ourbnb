using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ourbnb.Models;

namespace Ourbnb.ViewModels
{
    public class CreateOrder
    {
        public Order Order { get; set; } = default!;
        public List<SelectListItem> CustomerList { get; set; } = default!;
        public List<SelectListItem> RentalList { get; set; } = default!;
    }
}
