using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Domain.Users;
using PersonalShop.Features.Identitys.Users.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Resources.Services.UserServices;

namespace PersonalShop.Features.Identitys.Users;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ServiceResult<string>> CreateUserAsync(RegisterDto registerDto)
    {
        var userExistCheck = await _userManager.FindByEmailAsync(registerDto.Email);

        if (userExistCheck is not null)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.UserEmailExist);
        }

        var user = User.CreateNewWithFullNameAndPhoneNumber(registerDto.Email, registerDto.UserName,
            registerDto.FirstName, registerDto.LastName, registerDto.PhoneNumber);

        var userCreateResult = await _userManager.CreateAsync(user, registerDto.Password);

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
    public async Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.UserNotFound);
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
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

