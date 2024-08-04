using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PersonalShop.Domain.Users.Dtos;
using PersonalShop.Interfaces;

namespace PersonalShop.Api;

public static class AccountApis
{
    public static void RegisterAccountApis(this WebApplication app)
    {
        app.MapPost("Api/Account/Register", async ([FromBody] RegisterDto registerDto, IUserService userService) =>
        {
            if (await userService.CreateUserAsync(registerDto.UserName,
                                              registerDto.Password,
                                              registerDto.Email,
                                              registerDto.FirstName,
                                              registerDto.LastName,
                                              registerDto.PhoneNumber))
            {
                return Results.Ok("User registered successfully");
            }

            return Results.Ok("Problem in register user");
        });

        app.MapPost("Api/Account/Login", async ([FromBody] LoginDto loginModel, IAuthenticationService authenticationService, IUserService userService) =>
        {
            var user = await authenticationService.LoginAsync(loginModel.Email, loginModel.Password);
            if (user is not null)
            {
                var tokenString = userService.CreateTokenAsync(user);
                return Results.Ok(tokenString);
            }

            return Results.BadRequest("Problem in login user to website");
        });

        app.MapPost("Api/Account/Logout", async (IAuthenticationService authenticationService,HttpContext context) =>
        {
            string? token = context.Request.Headers[HeaderNames.Authorization];

            if (string.IsNullOrEmpty(token))
            {
                return Results.BadRequest("Problem to get jwt token in request header");
            }

            token = token.Replace("Bearer ", string.Empty);

            if (await authenticationService.LogoutAsync(token))
            {
                return Results.Ok("Your account logged out successfully");
            }

            return Results.BadRequest("Problem in logged out account");
        }).RequireAuthorization();
    }
}
