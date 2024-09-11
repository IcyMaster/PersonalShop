using Microsoft.AspNetCore.Http.HttpResults;
using PersonalShop.Extension;
using PersonalShop.Features.Cart;

namespace PersonalShop.Api;

public static class CartApis
{
    public static void RegisterCardApis(this WebApplication app)
    {
        app.MapGet("Api/Cart", async (ICartService cardService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Results.BadRequest("Please Login Again ...");
            }

            return Results.Ok(await cardService.GetCartByUserIdAsync(userId));

        }).RequireAuthorization();
    }
}
