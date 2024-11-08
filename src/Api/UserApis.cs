using Microsoft.AspNetCore.Authorization;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Orders.Dtos;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class UserApis
{
    public static void RegisterUserApis(this WebApplication app)
    {
        app.MapGet("api/user/products", [Authorize(Roles = RolesContract.Customer)] async (PagedResultOffset resultOffset, IProductService productService, HttpContext context) =>
        {
            var validateObject = ObjectValidatorExtension.Validate(resultOffset);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await productService.GetAllProductsWithUserAndValidateOwnerAsync(userId!, resultOffset);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<PagedResult<SingleProductDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<PagedResult<SingleProductDto>>.Failed(serviceResult.Errors));
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
