using EasyCaching.Core;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;
using PersonalShop.Domain.Users;
using PersonalShop.Interfaces.Features;
using System.IdentityModel.Tokens.Jwt;

namespace PersonalShop.Features.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<Domain.Users.User> _signInManager;
    private readonly UserManager<Domain.Users.User> _userManager;
    private readonly IEasyCachingProviderFactory _cachingfactory;

    public AuthenticationService(SignInManager<Domain.Users.User> signInManager, UserManager<Domain.Users.User> userManager, IEasyCachingProviderFactory cachingfactory)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _cachingfactory = cachingfactory;
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
    public async Task<bool> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return true;
    }
    public async Task<bool> LogoutAsync(string jwtToken)
    {
        var jwtBlackList = _cachingfactory.GetCachingProvider("JwtBlackList");

        if(await jwtBlackList.GetCountAsync($"JwtBlackList:{jwtToken}") > 0)
        {
            return true;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

        var exp = jwtSecurityToken.Claims.FirstOrDefault(e => e.Type.Equals("exp"));

        if (exp is not null)
        {
            if (long.TryParse(exp.Value, out var unixExp))
            {
                var expireTime = DateTimeOffset.FromUnixTimeSeconds(unixExp).LocalDateTime;
                if (expireTime < DateTime.Now)
                {
                    return true;
                }
                else
                {
                    await jwtBlackList.SetAsync($"JwtBlackList:{jwtToken}",string.Empty, TimeSpan.FromTicks(unixExp));
                    return true;
                }
            }
        }

        return false;
    }
}

