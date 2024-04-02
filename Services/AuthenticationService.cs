using Microsoft.AspNetCore.Identity;
using Personal_Shop.Interfaces;
using Personal_Shop.Models.Identity;

namespace Personal_Shop.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<CustomUser> _signInManager;
    private readonly UserManager<CustomUser> _userManager;

    public AuthenticationService(SignInManager<CustomUser> signInManager, UserManager<CustomUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return false;
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);

        if (!result.Succeeded)
        {
            return false;
        }

        return true;
    }
}

