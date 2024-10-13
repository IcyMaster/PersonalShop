using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Domain.Carts.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Api;

public static class CartApis
{
    public static void RegisterCardApis(this WebApplication app)
    {
        app.MapGet("Api/Cart", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            return Results.Ok(await cartService.GetCartByUserIdWithProductAsync(userId!));

        });

        app.MapPost("Api/Cart/AddItem", [Authorize(Roles = RolesContract.Customer)] async ([FromBody] CreateCartItemDto createCartItemDto, ICartService cartService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            var validateRes = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createCartItemDto, new ValidationContext(createCartItemDto), validateRes, true))
            {
                return Results.BadRequest(validateRes.Select(e => e.ErrorMessage));
            }

            if (await cartService.AddCartItemByUserIdAsync(userId!, createCartItemDto.ProductId, createCartItemDto.Quanity))
            {
                return Results.Ok("Item added to cart Succesfully");
            }

            return Results.BadRequest("Problem to add item to cart ...");

        });

        app.MapDelete("Api/Cart/DeleteItem/{productId:int}", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            if (await cartService.DeleteCartItemByUserIdAsync(userId!, productId))
            {
                return Results.Ok("Item deleted from cart Succesfully");
            }

            return Results.BadRequest("Problem delete item from cart ...");

        });

        app.MapPut("Api/Cart/UpdateItem/{productId:int}", [Authorize(Roles = RolesContract.Customer)] async ([FromBody] UpdateCartItemDto updateCartItemDto, ICartService cartService, HttpContext context, int productId) =>
        {
            var userId = context.GetUserId();

            if (await cartService.UpdateCartItemQuanityByUserIdAsync(userId!, productId, updateCartItemDto.Quanity))
            {
                return Results.Ok("The item info in the shopping cart has been updated successfully");
            }

            return Results.BadRequest("Problem to update cart item ...");

        });

        //just complete checkout without process payment
        app.MapGet("Api/Cart/Checkout/{cartId:Guid}", [Authorize(Roles = RolesContract.Customer)] async (ICartService cartService, IOrderService orderService, Guid cartId) =>
        {
            if (await orderService.CreateOrderByCartIdAsync(cartId))
            {
                return Results.Ok("Payment has been made successfully");
            }

            return Results.BadRequest("Problem processing payment ...");

        });
    }
}
