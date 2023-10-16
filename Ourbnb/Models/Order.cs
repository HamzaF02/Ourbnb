﻿namespace Ourbnb.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int? Rating { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public int RentalId { get; set; }
        public Rental Rental { get; set;} = default!;
        public int TotalPrice { get; set; }
    }
}
