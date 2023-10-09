using MyShop.Models;

namespace Ourbnb.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int RentalId { get; set; }
        public Rental Rental { get; set;}
        public int totalPrice { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int Rating { get; set; }
    }
}
