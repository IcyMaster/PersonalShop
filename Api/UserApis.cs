using PersonalShop.Interfaces;
using System.Security.Claims;

namespace PersonalShop.Api;

public static class UserApis
{
    public static void RegisterUserApis(this WebApplication app)
    {
        app.MapGet("Api/User/Products",async (IProductService productService, HttpContext context) =>
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;
            return Results.Ok(await productService.GetProducts(userId));
        }).RequireAuthorization();
    }
}
