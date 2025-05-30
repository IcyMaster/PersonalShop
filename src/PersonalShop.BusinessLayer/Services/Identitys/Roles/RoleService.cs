﻿using Microsoft.AspNetCore.Identity;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.Domain.Entities.Roles;

namespace PersonalShop.BusinessLayer.Services.Identitys.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<UserRole> _roleManager;

    public RoleService(RoleManager<UserRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        var userRole = new UserRole(roleName);

        var result = await _roleManager.CreateAsync(userRole);

        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }
    public async Task<bool> CheckRoleExistAsync(string roleName)
    {
        var userRole = await _roleManager.FindByNameAsync(roleName);

        if (userRole is not null)
        {
            return true;
        }

        return false;
    }
}
