﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PersonalShop.BusinessLayer.Services.Identitys.Authentications.Dtos;
using PersonalShop.BusinessLayer.Services.Identitys.Roles.Dtos;
using PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Extension;
using PersonalShop.Shared.Contracts;
using PersonalShop.Shared.Resources.Services.AuthenticationService;

namespace PersonalShop.Api;

public static class AccountApis
{
    public static void RegisterAccountApis(this WebApplication app)
    {
        app.MapPost("api/account/register", [AllowAnonymous] async ([FromBody] RegisterDto registerDto, IUserService userService) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(registerDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await userService.CreateUserAsync(registerDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPost("api/account/login", [AllowAnonymous] async ([FromBody] LoginDto loginDto, IAuthenticationService authenticationService) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(loginDto);
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

        app.MapPost("api/account/logout", [Authorize] async (IAuthenticationService authenticationService, HttpContext context) =>
        {
            string? token = context.Request.Headers[HeaderNames.Authorization];
            var userId = context.GetUserId();

            if (string.IsNullOrEmpty(token))
            {
                return Results.BadRequest(ApiResult<string>.Failed(AuthenticationServiceErrors.GetJwtTokenProblem));
            }

            var serviceResult = await authenticationService.LogoutApiAsync(token, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapGet("api/account/roles", [Authorize] async (IUserService userService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await userService.GetUserRolesByIdAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<string>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPost("api/account/roles/assignRole", [Authorize(Roles = RolesContract.Owner)] async ([FromBody] AssignRoleDto assignRoleDto, IUserService userService) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(assignRoleDto);
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

        app.MapDelete("api/account/roles/removeRole", [Authorize(Roles = RolesContract.Owner)] async ([FromBody] RemoveRoleDto removeRoleDto, IUserService userService) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(removeRoleDto);
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
