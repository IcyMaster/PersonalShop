using PersonalShop.Data.Contracts;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Configuration;

public static class Seeder
{
    public static async void SeedRolesAndOwnerUserAsync(this WebApplication app)
    {
        var serviceScope = app.Services.CreateScope();

        var services = serviceScope.ServiceProvider;

        var roleService = services.GetRequiredService<IRoleService>();
        var userService = services.GetRequiredService<IUserService>();

        const string userEmail = "icyMaster2020@gmail.com";
        const string firstName = "Mohammad";
        const string lastName = "Taheri";
        const string userName = "icyMaster";
        const string userPassword = "123@456#Pass";
        const string phoneNumber = "09902264112";

        if (await userService.CheckUserExistAsync(userEmail))
        {
            return;
        }

        foreach(var roles in RolesContract.Roles)
        {
            if(!await roleService.CheckRoleExistAsync(roles))
            {
                await roleService.CreateRoleAsync(roles);
            }
        }

        await userService.CreateUserAsync(userName, userPassword, userEmail,firstName
            ,lastName,phoneNumber);

        await userService.AssignUserRoleAsync(userEmail, RolesContract.Owner);
    }
}
