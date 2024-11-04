using Microsoft.EntityFrameworkCore;
using PersonalShop.Data;
using PersonalShop.Data.Contracts;

namespace PersonalShop.Configuration;

public static class Migrations
{
    public static async void MigrateApplicationSeeder(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();

        var seeders = serviceScope.ServiceProvider.GetServices<IDataBaseSeeder>();

        Console.WriteLine("Starting Seeding database");

        foreach (var seeder in seeders)
        {
            if (!await seeder.MigrateAsync())
            {
                Console.WriteLine("Failed to migrate database");
                await app.StopAsync();
            }
        }

        Console.WriteLine("Seeding database done !");
    }

    public static async void ApplyMigrations(this WebApplication app)
    {
        await using var serviceScope = app.Services.CreateAsyncScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Console.WriteLine("Starting Migration database");

        var checkMigration = await dbContext.Database.GetPendingMigrationsAsync();

        if (checkMigration.Any())
        {
            Console.WriteLine("Migration database done !");
            return;
        }

        await dbContext.Database.MigrateAsync();

        Console.WriteLine("Migration database done !");
    }
}
