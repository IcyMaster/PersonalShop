using Microsoft.AspNetCore.Identity;

namespace PersonalShop.Domain.Entities.Roles;

public class UserRole : IdentityRole
{
    public UserRole() : base()
    {
    }

    public UserRole(string roleName) : base(roleName)
    {

    }
}
