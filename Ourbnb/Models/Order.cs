using System.ComponentModel.DataAnnotations;

namespace Ourbnb.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be greater than 0 and 5 or less")]
        public int? Rating { get; set; }
        public int CustomerId { get; set; } = default!;
        public virtual Customer Customer { get; set; } = default!;
        public int RentalId { get; set; }
        public virtual Rental Rental { get; set;} = default!;
        public int TotalPrice { get; set; }
    }
}
