using Microsoft.AspNetCore.Authorization;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Orders.Dtos;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Domain.Response;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class UserApis
{
    public static void RegisterUserApis(this WebApplication app)
    {
        app.MapGet("Api/User/Products", [Authorize(Roles = RolesContract.Customer)] async (IProductService productService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await productService.GetAllProductsWithUserAndValidateOwnerAsync(userId!);

            if(serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleProductDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleProductDto>>.Failed(serviceResult.Errors));
        });

        app.MapGet("Api/User/Orders", [Authorize(Roles = RolesContract.Customer)] async (IOrderService orderService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await orderService.GetAllOrderByUserIdAsync(userId!);

            if(serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleOrderDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleOrderDto>>.Failed(serviceResult.Errors));
        });
    }
}
