using PersonalShop.Interfaces;

namespace PersonalShop.Api;

public static class CardApis
{
    public static void RegisterCardApis(this WebApplication app)
    {
        app.MapGet("Api/Card", async (ICardService cardService) => await cardService.GetCardItems()).AllowAnonymous();
    }
}
