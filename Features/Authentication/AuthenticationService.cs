using Microsoft.AspNetCore.Identity;
using PersonalShop.Domain.Users;
using PersonalShop.Interfaces;

namespace PersonalShop.Features.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<Domain.Users.User> _signInManager;
    private readonly UserManager<Domain.Users.User> _userManager;

    public AuthenticationService(SignInManager<Domain.Users.User> signInManager, UserManager<Domain.Users.User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Domain.Users.User?> LoginAsync(string email, string password)
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

