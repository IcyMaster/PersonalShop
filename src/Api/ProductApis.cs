using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Domain.Response;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class ProductApis
{
    public static void RegisterProductApis(this WebApplication app)
    {
        app.MapGet("Api/Products", [AllowAnonymous] async (IProductService productService) =>
        {
            var serviceResult = await productService.GetAllProductsWithUserAsync();

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleProductDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleProductDto>>.Failed(serviceResult.Errors));
        });

        app.MapPost("Api/Products/AddProduct", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] CreateProductDto createProductDto, IProductService productService, HttpContext context) =>
        {
            var validateObject = ObjectValidator.Validate(createProductDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await productService.CreateProductByUserIdAsync(createProductDto, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapGet("Api/Products/{productId:int}", [AllowAnonymous] async (IProductService productService, int productId) =>
        {
            var serviceResult = await productService.GetProductByIdWithUserAsync(productId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<SingleProductDto>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPut("Api/Products/UpdateProduct/{productId:int}", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] UpdateProductDto updateProductDto, IProductService productService, HttpContext context, int productId) =>
        {
            var validateObject = ObjectValidator.Validate(updateProductDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await productService.UpdateProductByIdAndValidateOwnerAsync(productId, updateProductDto, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapDelete("Api/Products/DeleteProduct/{productId:int}", [Authorize(Roles = RolesContract.Admin)] async (IProductService productService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await productService.DeleteProductByIdAndValidateOwnerAsync(productId, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
