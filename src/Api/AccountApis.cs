using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Authentication.Dtos;
using PersonalShop.Domain.Response;
using PersonalShop.Domain.Roles.Dtos;
using PersonalShop.Domain.Users.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using PersonalShop.Resources.Services.AuthenticationService;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Api;

public static class AccountApis
{
    public static void RegisterAccountApis(this WebApplication app)
    {
        app.MapPost("Api/Account/Register", [AllowAnonymous] async ([FromBody] RegisterDto registerDto, IUserService userService) =>
        {
            var validateObject = ObjectValidator.Validate(registerDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await userService.CreateUserAsync(registerDto.UserName,
                                              registerDto.Password,
                                              registerDto.Email,
                                              registerDto.FirstName,
                                              registerDto.LastName,
                                              registerDto.PhoneNumber);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPost("Api/Account/Login", [AllowAnonymous] async ([FromBody] LoginDto loginDto, IAuthenticationService authenticationService) =>
        {
            var validateObject = ObjectValidator.Validate(loginDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await authenticationService.LoginAsyncAndCreateToken(loginDto.Email, loginDto.Password);
            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<TokenDto>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPost("Api/Account/Logout", [Authorize] async (IAuthenticationService authenticationService, HttpContext context) =>
        {
            string? token = context.Request.Headers[HeaderNames.Authorization];

            if (string.IsNullOrEmpty(token))
            {
                return Results.BadRequest(ApiResult<string>.Failed(AuthenticationServiceErrors.GetJwtTokenProblem));
            }

            var serviceResult = await authenticationService.LogoutApiAsync(token);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapGet("Api/Account/Roles", [Authorize] async (IUserService userService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await userService.GetUserRolesByIdAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<string>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPost("Api/Account/Roles/AssignRole", [Authorize(Roles = RolesContract.Owner)] async ([FromBody] AssignRoleDto assignRoleDto, IUserService userService) =>
        {
            var validateObject = ObjectValidator.Validate(assignRoleDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await userService.AssignUserRoleByEmailAsync(assignRoleDto.UserEmail, assignRoleDto.RoleName);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapDelete("Api/Account/Roles/RemoveRole", [Authorize(Roles = RolesContract.Owner)] async ([FromBody] RemoveRoleDto removeRoleDto, IUserService userService) =>
        {
            var validateObject = ObjectValidator.Validate(removeRoleDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await userService.RemoveUserRoleByEmailAsync(removeRoleDto.UserEmail, removeRoleDto.RoleName);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
