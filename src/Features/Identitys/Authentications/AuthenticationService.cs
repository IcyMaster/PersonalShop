using EasyCaching.Core;
using Microsoft.AspNetCore.Identity;
using PersonalShop.Builders.Caches;
using PersonalShop.Domain.Responses;
using PersonalShop.Domain.Users;
using PersonalShop.Features.Identitys.Authentications.Dtos;
using PersonalShop.Interfaces.Features;
using PersonalShop.Interfaces.Generator;
using PersonalShop.Resources.Services.AuthenticationService;
using System.IdentityModel.Tokens.Jwt;

namespace PersonalShop.Features.Identitys.Authentications;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEasyCachingProvider _cachingProvider;

    public AuthenticationService(SignInManager<User> signInManager, UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator, IEasyCachingProvider cachingProvider)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _cachingProvider = cachingProvider;
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
    public async Task<ServiceResult<User>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return ServiceResult<User>.Failed(AuthenticationServiceErrors.UserNotFound);
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, true, false);

        if (!result.Succeeded)
        {
            return ServiceResult<User>.Failed(AuthenticationServiceErrors.UserNotFound);
        }

        return ServiceResult<User>.Success(user); ;
    }
    public async Task<ServiceResult<string>> LogoutApiAsync(string jwtToken, string userId)
    {
        jwtToken = jwtToken.Replace("Bearer ", string.Empty);

        string cacheKey = JwtCacheKeyBuilder.JwtCacheKeyWithUserId(userId);

        var cache = await _cachingProvider.GetAsync<string>(cacheKey);

        if (cache.HasValue)
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
                    await _cachingProvider.TrySetAsync(cacheKey, jwtToken, TimeSpan.FromTicks(unixExp));
                    return ServiceResult<string>.Success(AuthenticationServiceSuccess.SuccessfulLogout);
                }
            }
        }

        return ServiceResult<string>.Success(AuthenticationServiceErrors.LogoutProblem);
    }
    public async Task<ServiceResult<string>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return ServiceResult<string>.Success(AuthenticationServiceSuccess.SuccessfulLogout);
    }
}

