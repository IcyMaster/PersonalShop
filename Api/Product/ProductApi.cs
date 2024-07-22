using Microsoft.AspNetCore.Authorization;
using Personal_Shop.Interfaces;

namespace Personal_Shop.Api.Product
{
    public static class ProductApi
    {
        public static void SetupProductApi(this WebApplication app)
        {
            app.MapGet("/Products", async (IProductService productService) => await productService.GetProducts()).RequireAuthorization();
        }
    }
}
