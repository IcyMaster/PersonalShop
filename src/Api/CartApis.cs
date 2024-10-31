using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Domain.Carts.Dtos;
using PersonalShop.Domain.Response;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Api;

public static class CartApis
{
    public static void RegisterCardApis(this WebApplication app)
    {
        app.MapGet("Api/Cart", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await cartService.GetCartByUserIdWithProductAsync(userId!);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<SingleCartDto>.Success(serviceResult.Result!));
            }

            return Results.Ok(ApiResult<SingleCartDto>.Failed(serviceResult.Errors));
        });

        app.MapPost("Api/Cart/AddItem", [Authorize(Roles = RolesContract.Customer)] async ([FromBody] CreateCartItemDto createCartItemDto, ICartService cartService, HttpContext context) =>
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

        app.MapDelete("Api/Cart/DeleteItem/{productId:int}", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            var serviceResult = await cartService.DeleteCartItemByUserIdAsync(userId!, productId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });

        app.MapPut("Api/Cart/UpdateItem/{productId:int}", [Authorize(Roles = RolesContract.Customer)] async ([FromBody] UpdateCartItemDto updateCartItemDto, ICartService cartService, HttpContext context, int productId) =>
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
        app.MapGet("Api/Cart/Checkout/{cartId:Guid}", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, IOrderService orderService, Guid cartId) =>
        {
            var serviceResult = await orderService.CreateOrderByCartIdAsync(cartId);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
