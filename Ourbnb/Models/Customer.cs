
using System.ComponentModel.DataAnnotations;

namespace Ourbnb.Models
{

    public class Customer
    {
        public int CustomerId { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ]{1,20}", ErrorMessage = "First Name must be letters between 1 to 20 charachters")]
        public string FirstName { get; set; } = string.Empty;

        [RegularExpression(@"[a-zA-ZæøåÆØÅ]{1,20}", ErrorMessage = "Last Name must be letters between 1 to 20 charachters")]
        public string LastName { get; set; } = string.Empty;

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{1,50}", ErrorMessage = "Adress must be letters and numbers between 1 to 50 charachters")]
        public string Address { get; set; } = string.Empty;

        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Phone number must be 8 numbers")]
        public int Phone { get; set; }

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{1,50}", ErrorMessage = "Email must be letters and numbers between 1 to 50 charachters")]
        public string Email { get; set; } = string.Empty;

        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{1,50}", ErrorMessage = "Password must be letters and numbers between 1 to 50 charachters")]
        public string Password { get; set; } = string.Empty;

        public virtual List<Order>? Orders { get; set; }
    }
}

