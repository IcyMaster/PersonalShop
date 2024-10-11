using Microsoft.AspNetCore.Authorization;
using PersonalShop.Data.Contracts;
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
            return Results.Ok(await productService.GetAllProductsWithUserAndValidateOwnerAsync(userId!));

        });

        app.MapGet("Api/User/Orders", [Authorize(Roles = RolesContract.Customer)] async (IOrderService orderService, HttpContext context) =>
        {
            var userId = context.GetUserId();
            return Results.Ok(await orderService.GetAllOrderByUserIdAsync(userId!));
        });
    }
}
