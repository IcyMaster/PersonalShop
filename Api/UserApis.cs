using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using System.Security.Claims;

namespace PersonalShop.Api;

public static class UserApis
{
    public static void RegisterUserApis(this WebApplication app)
    {
        app.MapGet("Api/User/Products",async (IProductService productService, HttpContext context) =>
        {
            var userId = context.GetUserId();
            return Results.Ok(await productService.GetProducts(userId!));

        }).RequireAuthorization();

        app.MapGet("Api/User/Orders", async (IOrderService orderService, HttpContext context) =>
        {
            var userId = context.GetUserId();
            return Results.Ok(await orderService.GetAllOrderByUserId(userId!));
        }).RequireAuthorization();
    }
}
