using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ourbnb.Models;

namespace Ourbnb.ViewModels
{
    public class CreateCustomer
    {
        public Customer customer { get; set; } = default!;
    }
}