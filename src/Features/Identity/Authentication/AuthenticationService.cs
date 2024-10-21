using EasyCaching.Core;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;
using PersonalShop.Domain.Authentication.Dtos;
using PersonalShop.Domain.Response;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Generator;
using PersonalShop.Resources.Services.AuthenticationService;
using System.IdentityModel.Tokens.Jwt;

namespace PersonalShop.Features.Identity.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<Domain.Users.User> _signInManager;
    private readonly UserManager<Domain.Users.User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEasyCachingProviderFactory _cachingfactory;

    public AuthenticationService(SignInManager<Domain.Users.User> signInManager, UserManager<Domain.Users.User> userManager, IJwtTokenGenerator jwtTokenGenerator, IEasyCachingProviderFactory cachingfactory)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _cachingfactory = cachingfactory;
    }

    public async Task<ServiceResult<TokenDto>> LoginAsyncAndCreateToken(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return ServiceResult<TokenDto>.Failed(AuthenticationServiceErrors.UserNotFound);
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);

        if (!result.Succeeded)
        {
            return ServiceResult<TokenDto>.Failed(AuthenticationServiceErrors.WrongEmailOrPassword);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var tokenDto = new TokenDto(_jwtTokenGenerator.GenerateToken(roles.ToList(), user.Id));

        return ServiceResult<TokenDto>.Success(tokenDto);
    }
    public async Task<ServiceResult<Domain.Users.User>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return ServiceResult<Domain.Users.User>.Failed(AuthenticationServiceErrors.UserNotFound);
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);

        if (!result.Succeeded)
        {
            return ServiceResult<Domain.Users.User>.Failed(AuthenticationServiceErrors.UserNotFound);
        }

        return ServiceResult<Domain.Users.User>.Success(user); ;
    }
    public async Task<ServiceResult<string>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return ServiceResult<string>.Success(AuthenticationServiceSuccess.SuccessfulLogout);
    }
    public async Task<ServiceResult<string>> LogoutApiAsync(string jwtToken)
    {
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);

        var jwtBlackList = _cachingfactory.GetCachingProvider("JwtBlackList");

        if (await jwtBlackList.GetCountAsync($"JwtBlackList:{jwtToken}") > 0)
        {
            return ServiceResult<string>.Success(AuthenticationServiceSuccess.SuccessfulLogout);
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
                    return ServiceResult<string>.Success(AuthenticationServiceSuccess.SuccessfulLogout);
                }
                else
                {
                    await jwtBlackList.SetAsync($"JwtBlackList:{jwtToken}", string.Empty, TimeSpan.FromTicks(unixExp));
                    return ServiceResult<string>.Success(AuthenticationServiceSuccess.SuccessfulLogout);
                }
            }
        }

        return ServiceResult<string>.Success(AuthenticationServiceErrors.LogoutProblem);
    }
}

