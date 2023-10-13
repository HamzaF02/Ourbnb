namespace Ourbnb.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Phone { get; set; }
    public string Email { get; set; } = string.Empty;
}

