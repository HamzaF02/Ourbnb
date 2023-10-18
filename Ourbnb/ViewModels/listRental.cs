using System;
using System.Collections.Generic;
using Ourbnb.Models;

namespace MyShop.ViewModels
{
    public class listRental
    {
        public IEnumerable<Rental> Items;
        public string? CurrentViewName;

        public listRental(IEnumerable<Rental> items, string? currentViewName)
        {
            Items = items;
            CurrentViewName = currentViewName;
        }
    }
}
