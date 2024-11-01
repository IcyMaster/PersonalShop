using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Carts.Dtos;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class CartApis
{
    public static void RegisterCardApis(this WebApplication app)
    {
        app.MapGet("api/cart", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await cartService.GetCartByUserIdWithProductAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<SingleCartDto>.Success(serviceResult.Result!));
            }

            return Results.Ok(ApiResult<SingleCartDto>.Failed(serviceResult.Errors));
        });

        app.MapPost("api/cart", [Authorize(Roles = RolesContract.Customer)] async ([FromBody] CreateCartItemDto createCartItemDto, ICartService cartService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var validateObject = ObjectValidator.Validate(createCartItemDto);
            if (!validateObject.IsValid)
            {
                return Results.BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
            }

            var serviceResult = await cartService.AddCartItemByUserIdAsync(userId!, createCartItemDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapDelete("api/cart/{productId:int}", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await cartService.DeleteCartItemByUserIdAsync(userId!, productId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPut("api/cart/{productId:int}", [Authorize(Roles = RolesContract.Customer)] async ([FromBody] UpdateCartItemDto updateCartItemDto, ICartService cartService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await cartService.UpdateCartItemQuantityByUserIdAsync(userId!, productId, updateCartItemDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        //just complete checkout without process payment
        app.MapPost("api/cart/checkout", [Authorize(Roles = RolesContract.Customer)] async (IOrderService orderService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await orderService.CreateOrderByUserIdAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
