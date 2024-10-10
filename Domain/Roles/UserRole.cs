using Microsoft.AspNetCore.Identity;

namespace PersonalShop.Domain.Roles;

public class UserRole : IdentityRole<Guid>
{
    public UserRole(string roleName)
    {
        base.Name = roleName;
    }
}
