using Microsoft.AspNetCore.Identity;
using PersonalShop.Domain.Products.DTO;

namespace PersonalShop.Domain.Users;

public class CustomUser : IdentityUser
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public List<ProductDTO>? Products { get; set; }

    public void SetPersonalName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
