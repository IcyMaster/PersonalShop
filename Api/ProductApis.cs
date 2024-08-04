using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Interfaces;
using System.Security.Claims;

namespace PersonalShop.Api
{
    public static class ProductApis
    {
        public static void RegisterProductApis(this WebApplication app)
        {
            app.MapGet("Api/Products", async (IProductService productService) => await productService.GetProducts()).AllowAnonymous();

            app.MapPost("Api/Products/AddProduct", async ([FromBody] CreateProductDto createProductDto, IProductService productService, HttpContext context) =>
            {
                var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;

                if (await productService.AddProduct(createProductDto, userId))
                {
                    return Results.Ok("Product added Succesfully");
                }

                return Results.BadRequest("Problem to add product in website ...");
            }).RequireAuthorization();

            app.MapGet("Api/Products/{productId:long}", async (IProductService productService, long productId) =>
            {
                var product = await productService.GetProductById(productId);
                if (product is null)
                {
                    return Results.BadRequest("Problem in Load product");
                }

                return Results.Ok(product);
            }).AllowAnonymous();

            app.MapPut("Api/Products/UpdateProduct/{productId:long}", async ([FromBody] UpdateProductDto updateProductDto, IProductService productService, HttpContext context, long productId) =>
            {
                var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;

                if (await productService.UpdateProductById(productId, updateProductDto, userId))
                {
                    return Results.Ok("Product Edited Succesfully");
                }

                return Results.BadRequest("Problem in edit product");
            }).RequireAuthorization();

            app.MapDelete("Api/Products/DeleteProduct/{productId:long}", async (IProductService productService, HttpContext context, long productId) =>
            {
                var userId = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value!;

                if (await productService.DeleteProductById(productId, userId))
                {
                    return Results.Ok("Product Deleted Succesfully");
                }

                return Results.Ok("Problem in Delete product");
            }).RequireAuthorization();
        }
    }
}
