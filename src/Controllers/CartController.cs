using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Carts.Dtos;
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

    [Authorize(Roles = RolesContract.Customer)]
    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var serviceResult = await _cartService.GetCartDetailsWithCartItemsAsync(User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [Authorize(Roles = RolesContract.Customer)]
    [HttpPost]
    [Route("AddItem")]
    public async Task<ActionResult> AddItem(CreateCartItemDto createCartItemDto)
    {
        var serviceResult = await _cartService.AddCartItemAsync(User.Identity!.GetUserId(), createCartItemDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [Authorize(Roles = RolesContract.Customer)]
    [HttpPost]
    [Route("DeleteItem/{productId:int}", Name = "DeleteItem")]
    public async Task<ActionResult> DeleteItem(int productId)
    {
        var serviceResult = await _cartService.DeleteCartItemAsync(User.Identity!.GetUserId(), productId);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [Authorize(Roles = RolesContract.Customer)]
    [HttpPost]
    [Route("UpdateItem/{productId:int}", Name = "UpdateItem")]
    public async Task<ActionResult> UpdateItem(int productId, UpdateCartItemDto updateCartItemDto)
    {
        var serviceResult = await _cartService.UpdateCartItemQuantityAsync(User.Identity!.GetUserId(), productId, updateCartItemDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [Authorize(Roles = RolesContract.Customer)]
    [HttpPost]
    [Route("Checkout", Name = "Checkout")]
    public async Task<ActionResult> CheckOut()
    {
        var serviceResult = await _orderService.CreateOrderAsync(User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(UserController.UserOrders), nameof(UserController).Replace("Controller", string.Empty));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}
