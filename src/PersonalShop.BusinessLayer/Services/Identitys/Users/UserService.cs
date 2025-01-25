using MassTransit.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Domain.Entities.Users;
using PersonalShop.Shared.Contracts;
using PersonalShop.Shared.Resources.Services.UserServices;

namespace PersonalShop.BusinessLayer.Services.Identitys.Users;

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
    public async Task<ServiceResult<PagedResult<SingleUserDto>>> GetAllUsersAsync(PagedResultOffset resultOffset)
    {
        var totalRecord = await _userManager.Users.CountAsync();

        var data = await _userManager.Users
            .AsNoTracking()
            .Skip((resultOffset.PageNumber - 1) * resultOffset.PageSize)
            .Take(resultOffset.PageSize)
            .Select(x => new SingleUserDto
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                User = x
            }).ToListAsync();

        foreach (var item in data)
        {
            var roles = await _userManager.GetRolesAsync(item.User);
            item.UserRoles = roles.ToList();
        }

        var result = PagedResult<SingleUserDto>.CreateNew(data, resultOffset, totalRecord);

        return ServiceResult<PagedResult<SingleUserDto>>.Success(result);
    }
    public async Task<ServiceResult<string>> AssignUserRoleByEmailAsync(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            return ServiceResult<string>.Failed(UserServiceErrors.UserNotFound);
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains(roleName))
        {
            return ServiceResult<string>.Failed(UserServiceErrors.RoleAlreadyAssigned);
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            return ServiceResult<string>.Success(UserServiceSuccess.SuccessfulAssineRoleToUser);
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

        var roles = await _userManager.GetRolesAsync(user);
        if(roles.Count is 1)
        {
            if (roles[0].Equals(roleName))
            {
                return ServiceResult<string>.Failed(UserServiceErrors.RemoveAllRolesProblem);
            }
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            return ServiceResult<string>.Success(UserServiceSuccess.SuccessfulRemoveRoleFromUser);
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
    public async Task<ServiceResult<List<string>>> GetUserRolesByEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null)
        {
            return ServiceResult<List<string>>.Failed(UserServiceErrors.UserNotFound);
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var result = userRoles.Select(x => x.ToString()).ToList();

        return ServiceResult<List<string>>.Success(result);
    }
    public async Task<ServiceResult<SingleUserDto>> GetUserByEmailAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user is null)
        {
            return ServiceResult<SingleUserDto>.Failed(UserServiceErrors.UserNotFound);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var singleUserDto = new SingleUserDto
        {
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            UserId = user.Id,
            User = user,
            UserRoles = roles.ToList(),
        };

        return ServiceResult<SingleUserDto>.Success(singleUserDto);
    }
}