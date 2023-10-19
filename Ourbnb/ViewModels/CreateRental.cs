using System;
using System.Collections.Generic;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ourbnb.Models;

namespace Ourbnb.ViewModels
{
    public class CreateRental
    {
        public Rental Rental { get; set; } = default!;
        public List<SelectListItem> OwnersList { get; set; } = default!;
    }
}
