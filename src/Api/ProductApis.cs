using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class ProductApis
{
    public static void RegisterProductApis(this WebApplication app)
    {
        app.MapGet("api/products", [AllowAnonymous] async (IProductService productService) =>
        {
            var serviceResult = await productService.GetAllProductsWithUserAsync();

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<List<SingleProductDto>>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<List<SingleProductDto>>.Failed(serviceResult.Errors));
        });

        app.MapPost("api/products", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] CreateProductDto createProductDto, IProductService productService, HttpContext context) =>
        {
            var validateObject = ObjectValidator.Validate(createProductDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await productService.CreateProductAsync(createProductDto, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapGet("api/products/{productId:int}", [AllowAnonymous] async (IProductService productService, int productId) =>
        {
            var serviceResult = await productService.GetProductDetailsWithUserAsync(productId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<SingleProductDto>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPut("api/products/{productId:int}", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] UpdateProductDto updateProductDto, IProductService productService, HttpContext context, int productId) =>
        {
            var validateObject = ObjectValidator.Validate(updateProductDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var userId = context.GetUserId();

            var serviceResult = await productService.UpdateProductAndValidateOwnerAsync(productId, updateProductDto, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapDelete("api/products/{productId:int}", [Authorize(Roles = RolesContract.Admin)] async (IProductService productService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await productService.DeleteProductAndValidateOwnerAsync(productId, userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
