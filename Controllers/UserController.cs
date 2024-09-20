using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Domain.Orders.Dtos;
using PersonalShop.Domain.Products.Dtos;
using PersonalShop.Extension;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Controllers;

[Route("User")]
public class UserController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public UserController(IProductService productService, IOrderService orderService)
    {
        _productService = productService;
        _orderService = orderService;
    }

    [Authorize]
    [HttpGet]
    [Route("Products", Name = "UserProducts")]
    public async Task<ActionResult<IEnumerable<SingleProductDto>>> UserProducts()
    {
        return View(await _productService.GetAllProductsWithUserAndValidateOwnerAsync(User.Identity!.GetUserId()));
    }

    [Authorize]
    [HttpGet]
    [Route("Orders", Name = "UserOrders")]
    public async Task<ActionResult<IEnumerable<SingleOrderDto>>> UserOrders()
    {
        return View(await _orderService.GetAllOrderByUserIdAsync(User.Identity!.GetUserId()));
    }
}
