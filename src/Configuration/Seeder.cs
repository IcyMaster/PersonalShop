using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Users.Dtos;
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

        const string email = "icyMaster2020@gmail.com";
        const string firstName = "Mohammad";
        const string lastName = "Taheri";
        const string userName = "icyMaster";
        const string password = "123@456#Pass";
        const string phoneNumber = "09902264112";

        if (await userService.CheckUserExistAsync(email))
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

        var user = new RegisterDto 
        {
            UserName = userName,
            Password = password,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
        };

        await userService.CreateUserAsync(user);

        await userService.AssignUserRoleByEmailAsync(email, RolesContract.Owner);
    }
}
