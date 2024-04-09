using Microsoft.AspNetCore.Identity;

namespace Personal_Shop.Models.Identity;

public class CustomUser : IdentityUser
{
    private string FirstName { get; set; } = string.Empty;
    private string LastName { get; set; } = string.Empty;

    public void SetPersonalName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
