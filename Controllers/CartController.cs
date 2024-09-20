using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Domain.Carts.Dtos;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Features.Product;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Controllers;

[Route("Cart")]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public CartController(ICartService cartService, IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> Index()
    {
        return View(await _cartService.GetCartByUserIdWithProductAsync(User.Identity!.GetUserId()));
    }

    [Authorize]
    [HttpPost]
    [Route("AddItem")]
    public async Task<ActionResult> AddItem(CreateCartItemDto createCartItemDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createCartItemDto);
        }

        if (await _cartService.AddCartItemByUserIdAsync(User.Identity!.GetUserId(), createCartItemDto.ProductId, createCartItemDto.Quanity))
        {
            return RedirectToAction(nameof(CartController.Index), "Cart");
        }

        return BadRequest("Problem in add item to cart ...");
    }

    [Authorize]
    [HttpPost]
    [Route("DeleteItem/{productId:int}", Name = "DeleteItem")]
    public async Task<ActionResult> DeleteItem(int productId)
    {
        if (!await _cartService.DeleteCartItemByUserIdAsync(User.Identity!.GetUserId(),productId))
        {
            return BadRequest("Problem delete item from cart ...");
        }

        return RedirectToAction(nameof(CartController.Index), "Cart");
    }

    [Authorize]
    [HttpPost]
    [Route("UpdateItem/{productId:int}", Name = "UpdateItem")]
    public async Task<ActionResult> UpdateItem(int productId,UpdateCartItemDto updateCartItemDto)
    {
        if (!await _cartService.UpdateCartItemQuanityByUserIdAsync(User.Identity!.GetUserId(), productId, updateCartItemDto.Quanity))
        {
            return BadRequest("Problem to update cart item ...");
        }

        return RedirectToAction(nameof(CartController.Index), "Cart");
    }

    [Authorize]
    [HttpGet]
    [Route("CheckOut/{cartId:Guid}", Name = "CheckOut")]
    public async Task<ActionResult> CheckOut(Guid cartId)
    {
        if (!await _orderService.CreateOrderByCartIdAsync(cartId))
        {
            return BadRequest("Problem processing payment ...");
        }

        return RedirectToAction(nameof(UserController.UserOrders), "User");
    }
}
