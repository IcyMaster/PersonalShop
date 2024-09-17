using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Api;

public static class ProductApis
{
    public static void RegisterProductApis(this WebApplication app)
    {
        app.MapGet("Api/Products", async (IProductService productService) => await productService.GetAllProductsWithUserAsync()).AllowAnonymous();

        app.MapPost("Api/Products/AddProduct", async ([FromBody] CreateProductDto createProductDto, IProductService productService, HttpContext context) =>
        {
            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createProductDto, new ValidationContext(createProductDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

            var userId = context.GetUserId();

            if (await productService.AddProductByUserIdAsync(createProductDto, userId!))
            {
                return Results.Ok("Product added Succesfully");
            }

            return Results.BadRequest("Problem to add product in website ...");

        }).RequireAuthorization();

        app.MapGet("Api/Products/{productId:int}", async (IProductService productService, int productId) =>
        {
            var product = await productService.GetProductByIdWithUserAsync(productId);
            if (product is null)
            {
                return Results.BadRequest("Problem in Load product");
            }

            return Results.Ok(product);

        }).AllowAnonymous();

        app.MapPut("Api/Products/UpdateProduct/{productId:int}", async ([FromBody] UpdateProductDto updateProductDto, IProductService productService, HttpContext context, int productId) =>
        {
            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(updateProductDto, new ValidationContext(updateProductDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

            var userId = context.GetUserId();

            if (await productService.UpdateProductByIdAndValidateOwnerAsync(productId, updateProductDto, userId!))
            {
                return Results.Ok("Product Edited Succesfully");
            }

            return Results.BadRequest("Problem in edit product");

        }).RequireAuthorization();

        app.MapDelete("Api/Products/DeleteProduct/{productId:int}", async (IProductService productService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            if (await productService.DeleteProductByIdAndValidateOwnerAsync(productId, userId!))
            {
                return Results.Ok("Product Deleted Succesfully");
            }

            return Results.Ok("Problem in Delete product");

        }).RequireAuthorization();
    }
}
