using Microsoft.AspNetCore.Identity;
using PersonalShop.Domain.Products;

namespace PersonalShop.Domain.Users;

public class User : IdentityUser
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    public void SetPersonalName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
