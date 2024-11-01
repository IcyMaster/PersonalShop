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
        app.MapGet("api/user/products", [Authorize(Roles = RolesContract.Customer)] async (IProductService productService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await productService.GetAllProductsWithUserAndValidateOwnerAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleProductDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleProductDto>>.Failed(serviceResult.Errors));
        });

        app.MapGet("api/user/orders", [Authorize(Roles = RolesContract.Customer)] async (IOrderService orderService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await orderService.GetAllOrderByUserIdAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleOrderDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleOrderDto>>.Failed(serviceResult.Errors));
        });
    }
}
