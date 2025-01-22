using Microsoft.AspNetCore.Authorization;
using PersonalShop.BusinessLayer.Services.Categories.Dtos;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.BusinessLayer.Services.Tags.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Extension;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Api;

public static class UserApis
{
    public static void RegisterUserApis(this WebApplication app)
    {
        app.MapGet("api/user/products", [Authorize(Roles = RolesContract.Admin)] async (PagedResultOffset resultOffset, IProductService productService, HttpContext context) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(resultOffset);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await productService.GetAllProductsWithUserAndValidateOwnerAsync(resultOffset,userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<PagedResult<SingleProductDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<PagedResult<SingleProductDto>>.Failed(serviceResult.Errors));
        });

        app.MapGet("api/user/tags", [Authorize(Roles = RolesContract.Admin)] async (PagedResultOffset resultOffset, ITagService tagService, HttpContext context) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(resultOffset);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await tagService.GetAllTagsWithUserAndValidateOwnerAsync(resultOffset,userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<PagedResult<SingleTagDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<PagedResult<SingleTagDto>>.Failed(serviceResult.Errors));
        });

        app.MapGet("api/user/categories", [Authorize(Roles = RolesContract.Admin)] async (PagedResultOffset resultOffset, ICategoryService categoryService, HttpContext context) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(resultOffset);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await categoryService.GetAllCategoriesWithUserAndValidateOwnerAsync(resultOffset, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<PagedResult<SingleCategoryDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<PagedResult<SingleCategoryDto>>.Failed(serviceResult.Errors));
        });

        app.MapGet("api/user/orders", [Authorize(Roles = RolesContract.Customer)] async (PagedResultOffset resultOffset, IOrderService orderService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await orderService.GetAllOrderAsync(userId!, resultOffset);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<PagedResult<SingleOrderDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<PagedResult<SingleOrderDto>>.Failed(serviceResult.Errors));
        });
    }
}
