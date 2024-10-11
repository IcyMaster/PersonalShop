using Microsoft.AspNetCore.Identity;

namespace PersonalShop.Domain.Roles;

public class UserRole : IdentityRole
{
    public UserRole() : base()
    {
    }

    public UserRole(string roleName) : base(roleName)
    {

    }
}
