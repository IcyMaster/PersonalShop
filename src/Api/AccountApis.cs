using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Roles.Dtos;
using PersonalShop.Domain.Users.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Api;

public static class AccountApis
{
    public static void RegisterAccountApis(this WebApplication app)
    {
        app.MapPost("Api/Account/Register",[AllowAnonymous] async ([FromBody] RegisterDto registerDto, IUserService userService) =>
        {
            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(registerDto, new ValidationContext(registerDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

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

        app.MapPost("Api/Account/Login",[AllowAnonymous] async ([FromBody] LoginDto loginDto, IAuthenticationService authenticationService, IUserService userService) =>
        {
            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(loginDto, new ValidationContext(loginDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

            var user = await authenticationService.LoginAsync(loginDto.Email, loginDto.Password);
            if (user is not null)
            {
                var tokenString = await userService.CreateTokenAsync(user);
                return Results.Ok(tokenString);
            }

            return Results.BadRequest("Problem in login user to website");
        });

        app.MapPost("Api/Account/Logout", [Authorize] async (IAuthenticationService authenticationService, HttpContext context) =>
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
        });

        app.MapGet("Api/Account/Roles", [Authorize] async (IUserService userService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var userRoles = await userService.GetUserRolesByIdAsync(userId!);

            if (userRoles is null)
            {
                return Results.BadRequest("Problem to get user roles");
            }

            return Results.Ok(userRoles);
        });

        app.MapPost("Api/Account/Roles/AssignRole", [Authorize(Roles = RolesContract.Owner)] async ([FromBody] AssignRoleDto assignRoleDto, IUserService userService) =>
        {
            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(assignRoleDto, new ValidationContext(assignRoleDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

            if (await userService.AssignUserRoleByEmailAsync(assignRoleDto.UserEmail, assignRoleDto.RoleName))
            {
                return Results.Ok("The role was successfully assigned");
            }

            return Results.BadRequest("Problem in assign Role to user");
        });

        app.MapDelete("Api/Account/Roles/RemoveRole", [Authorize(Roles = RolesContract.Owner)] async ([FromBody] RemoveRoleDto removeRoleDto, IUserService userService) =>
        {
            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(removeRoleDto, new ValidationContext(removeRoleDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

            if (await userService.RemoveUserRoleByEmailAsync(removeRoleDto.UserEmail, removeRoleDto.RoleName))
            {
                return Results.Ok("The role was successfully removed");
            }

            return Results.BadRequest("Problem in remove Role from user");
        });
    }
}
