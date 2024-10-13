using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Api;

public static class ProductApis
{
    public static void RegisterProductApis(this WebApplication app)
    {
        app.MapGet("Api/Products", [AllowAnonymous] async (IProductService productService) => await productService.GetAllProductsWithUserAsync());

        app.MapPost("Api/Products/AddProduct", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] CreateProductDto createProductDto, IProductService productService, HttpContext context) =>
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

        });

        app.MapGet("Api/Products/{productId:int}", [AllowAnonymous] async (IProductService productService, int productId) =>
        {
            var product = await productService.GetProductByIdWithUserAsync(productId);
            if (product is null)
            {
                return Results.BadRequest("Problem in Load product");
            }

            return Results.Ok(product);

        });

        app.MapPut("Api/Products/UpdateProduct/{productId:int}", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] UpdateProductDto updateProductDto, IProductService productService, HttpContext context, int productId) =>
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

        });

        app.MapDelete("Api/Products/DeleteProduct/{productId:int}", [Authorize(Roles = RolesContract.Admin)] async (IProductService productService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            if (await productService.DeleteProductByIdAndValidateOwnerAsync(productId, userId!))
            {
                return Results.Ok("Product Deleted Succesfully");
            }

            return Results.Ok("Problem in Delete product");

        });
    }
}
