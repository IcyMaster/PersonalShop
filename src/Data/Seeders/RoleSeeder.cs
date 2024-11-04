using Microsoft.AspNetCore.Identity;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Roles;

namespace PersonalShop.Data.Seeders;

internal class RoleSeeder : IDataBaseSeeder
{
    private readonly RoleManager<UserRole> _roleManager;
    private readonly ILogger<RoleSeeder> _logger;

    public RoleSeeder(RoleManager<UserRole> roleManager, ILogger<RoleSeeder> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<bool> MigrateAsync()
    {
        List<UserRole> unCreatedRoles = new List<UserRole>();

        foreach (var role in GetRoles())
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                unCreatedRoles.Add(new UserRole(role));
            }
        }

        if (unCreatedRoles.Count() is 0)
        {
            return true;
        }

        _logger.LogInformation("Role Seeder Migration Started!");

        try
        {
            foreach (var role in unCreatedRoles)
            {
                await _roleManager.CreateAsync(role);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error While Migration Role Seeder");
            return false;
        }

        _logger.LogInformation("Role Seeder Migration Has Finished!");
        return true;
    }

    private List<string> GetRoles()
    {
        return RolesContract.Roles;
    }
}
