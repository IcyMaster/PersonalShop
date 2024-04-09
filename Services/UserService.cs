using Microsoft.AspNetCore.Identity;
using Personal_Shop.Interfaces;
using Personal_Shop.Models.Identity;

namespace Personal_Shop.Services;

public class UserService : IUserService
{
    private readonly UserManager<CustomUser> _userManager;

    public UserService(UserManager<CustomUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> CreateUserAsync(string userName,
        string password,string email,string? firstName,
        string? lastName,string? phoneNumber)
    {
        var user = new CustomUser { UserName = userName, Email = email };

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
        {
            user.SetPersonalName(firstName, lastName);
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            user.PhoneNumber = phoneNumber;
        }

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return false;
        }

        return true;
    }

    public async Task<CustomUser?> GetCustomUserDetailAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        return user;
    }
}

