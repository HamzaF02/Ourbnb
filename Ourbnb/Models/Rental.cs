using MyShop.Models;

namespace Ourbnb.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double Rating { get; set; }
        public string Location { get; set; }
        public int Price { get; set; }

        public int OwnerId { get; set; }
        public Customer Owner { get; set; }

    }
}
