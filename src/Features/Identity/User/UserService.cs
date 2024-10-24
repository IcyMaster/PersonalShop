﻿using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Response;
using PersonalShop.Interfaces.Features;
using PersonalShop.Resources.Services.UserServices;

namespace PersonalShop.Features.Identity.User;

public class UserService : IUserService
{
    private readonly UserManager<Domain.Users.User> _userManager;

    public UserService(UserManager<Domain.Users.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ServiceResult<string>> CreateUserAsync(string userName,
        string password, string email, string? firstName,
        string? lastName, string? phoneNumber)
    {
        var user = new Domain.Users.User { UserName = userName, Email = email };

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            user.SetPersonalName(firstName, lastName);
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            user.PhoneNumber = phoneNumber;
        }

        var userExistCheck = await _userManager.FindByEmailAsync(email);
        if (userExistCheck is not null)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.UserEmailExist);
        }

        var userCreateResult = await _userManager.CreateAsync(user, password);

        if (!userCreateResult.Succeeded)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.RegisterAccountProblem);
        }

        var roleAssineResult = await _userManager.AddToRoleAsync(user, RolesContract.Customer);

        if (!roleAssineResult.Succeeded)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.AssineRoleToUserProblem);
        }

        return ServiceResult<string>.Success(UserServiceSuccess.SuccessfulRegisterAccount);
    }

    public async Task<bool> CheckUserExistAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is not null)
        {
            return true;
        }

        return false;
    }
    public async Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail,string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.UserNotFound);
        }

        var result = await _userManager.AddToRoleAsync(user,roleName);
        if (result.Succeeded)
        {
            return ServiceResult<string>.Failed(UserServiceSuccess.SuccessfulAssineRoleToUser);
        }

        return ServiceResult<string>.Failed(UserServiceErrors.AssineRoleToUserProblem);
    }
    public async Task<ServiceResult<string>> RemoveUserRoleByEmailAsync(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.UserNotFound);
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            return ServiceResult<string>.Success(UserServiceSuccess.SuccessfulAssineRoleToUser);
        }

        return ServiceResult<string>.Failed(UserServiceErrors.RemoveRoleFromUserProblem);
    }
    public async Task<ServiceResult<List<string>>> GetUserRolesByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return ServiceResult<List<string>>.Failed(UserServiceErrors.UserNotFound);
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = userRoles.Select(x => x.ToString()).ToList();

        return ServiceResult<List<string>>.Success(result);
    }
}

