using PersonalShop.Data.Contracts;
using PersonalShop.Features.Identitys.Users.Dtos;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Configuration;

public static class Migrations
{
    public static async void MigrateApplication(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();

        var seeders = serviceScope.ServiceProvider.GetServices<IDataBaseSeeder>();

        Console.WriteLine("Starting Seeding database");

        foreach (var seeder in seeders)
        {
            if(await seeder.MigrateAsync())
            {
                Console.WriteLine("Failed to migrate database");
                await app.StopAsync();
            }
        }

        Console.WriteLine("Seeding database done !");

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

        foreach (var roles in RolesContract.Roles)
        {
            if (!await roleService.CheckRoleExistAsync(roles))
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
