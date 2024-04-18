using Microsoft.AspNetCore.Identity;
using Personal_Shop.Models.Data;

namespace Personal_Shop.Models.Identity;

public class CustomUser : IdentityUser
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public List<Product> Products { get; set; } = new List<Product>();

    public void SetPersonalName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
