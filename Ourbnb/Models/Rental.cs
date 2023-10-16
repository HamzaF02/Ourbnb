using System.ComponentModel.DataAnnotations;

namespace Ourbnb.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public double Rating { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Bilder { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public Customer Owner { get; set; } = default!;

        public List<Order>? Orders { get; set; }
    }
}
