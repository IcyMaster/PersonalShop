using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Categories.Dtos;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class CategoryApis
{
    public static void RegisterCategoryApis(this WebApplication app)
    {
        app.MapGet("api/categories", [AllowAnonymous] async (ICategoryService categoryService) =>
        {
            var serviceResult = await categoryService.GetAllCategoriesWithUserAsync();

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleCategoryDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleCategoryDto>>.Failed(serviceResult.Errors));
        });

        app.MapPost("api/categories", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] CreateCategoryDto createCategoryDto, ICategoryService categoryService, HttpContext context) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(createCategoryDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await categoryService.CreateCategoryAsync(userId!, createCategoryDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPut("api/categories/{categoryId:int}", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] UpdateCategoryDto updateCategoryDto, ICategoryService categoryService, HttpContext context, int categoryId) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(updateCategoryDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await categoryService.UpdateCategoryAndValidateOwnerAsync(userId!, categoryId, updateCategoryDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapDelete("api/categories/{categoryId:int}", [Authorize(Roles = RolesContract.Admin)] async (ICategoryService categoryService, HttpContext context, int categoryId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await categoryService.DeleteCategoryAndValidateOwnerAsync(userId!, categoryId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
