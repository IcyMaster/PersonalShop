using Microsoft.AspNetCore.Identity;
using Personal_Shop.Domain.Users;
using Personal_Shop.Interfaces;

namespace Personal_Shop.Features.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<CustomUser> _signInManager;
    private readonly UserManager<CustomUser> _userManager;

    public AuthenticationService(SignInManager<CustomUser> signInManager, UserManager<CustomUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<CustomUser?> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);

        if (!result.Succeeded)
        {
            return null;
        }

        return user;
    }
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}

