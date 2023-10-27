using System.ComponentModel.DataAnnotations;

namespace Ourbnb.Models
{
    public class Rental
    {
        public int RentalId { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ]{2,30}", ErrorMessage = "Name must be letters or numbers between 2 to 30 charachters")]
        public string Name { get; set; } = string.Empty;
        [StringLength(200)]
        public string? Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [Range(0.01, 5, ErrorMessage ="Rating must be greater than 0 and 5 or less")]
        public double? Rating { get; set; } = default!;
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ ]{2,50}", ErrorMessage = "Name must be letters or numbers between 2 to 50 charachters")]
        public string Location { get; set; } = string.Empty;
        [Range(0.01, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public int Price { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Owners")]
        public int OwnerId { get; set; }
        public string IdentityId { get; set; } = string.Empty;
        public virtual Customer Owner { get; set; } = default!;
        public virtual List<Order>? Orders { get; set; }

        internal void UpdateRating()
        {
            if(Orders == null)
            {
                Rating = 0; return;
            }

            double rating = 0;
            foreach (var o in Orders)
            {
                if (o.Rating != null)
                {
                    rating += (int)o.Rating;
                }
            }
            Rating = rating / Orders.Count;
        }
    }
}
