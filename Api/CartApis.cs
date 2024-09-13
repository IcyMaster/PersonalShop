using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Api;

public static class CartApis
{
    public static void RegisterCardApis(this WebApplication app)
    {
        app.MapGet("Api/Cart", async (ICartService cartService, HttpContext context) =>
        {
            var userId = context.GetUserId();

            return Results.Ok(await cartService.GetCartByUserIdAsync(userId!));

        }).RequireAuthorization();

        app.MapPost("Api/Cart", async ([FromBody] CreateCartItemDto createCartItemDto, ICartService cartService, HttpContext context) =>
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
        }).RequireAuthorization();
    }
}
