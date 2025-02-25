using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.BusinessLayer.Services.Products.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Extension;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Controllers;

[Route("User")]
public class UserController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly ICategoryService _categoryService;

    public UserController(IProductService productService, IOrderService orderService, ICategoryService categoryService)
    {
        _productService = productService;
        _orderService = orderService;
        _categoryService = categoryService;
    }

    [HttpGet]
    [Route("Products", Name = "UserProducts")]
    [Authorize(Roles = RolesContract.Admin)]
    public async Task<ActionResult<PagedResult<SingleProductDto>>> UserProducts([FromQuery] PagedResultOffset resultOffset)
    {
        var validateObject = ObjectValidatorExtension.Validate(resultOffset);
        if (!validateObject.IsValid)
        {
            return BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
        }

        var serviceResult = await _productService.GetAllProductsWithUserAndValidateOwnerAsync(resultOffset,User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Route("Categories", Name = "UserCategories")]
    [Authorize(Roles = RolesContract.Admin)]
    public async Task<ActionResult<PagedResult<SingleProductDto>>> UserCategories([FromQuery] PagedResultOffset resultOffset)
    {
        var validateObject = ObjectValidatorExtension.Validate(resultOffset);
        if (!validateObject.IsValid)
        {
            return BadRequest(ApiResult<string>.Failed(validateObject.Errors!));
        }

        var serviceResult = await _categoryService.GetAllCategoriesWithUserAndValidateOwnerAsync(resultOffset, User.Identity!.GetUserId());

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [HttpGet]
    [Route("Orders", Name = "UserOrders")]
    [Authorize(Roles = RolesContract.Customer)]
    public async Task<ActionResult<PagedResult<SingleOrderDto>>> UserOrders([FromQuery] PagedResultOffset resultOffset)
    {
        var serviceResult = await _orderService.GetAllOrderAsync(User.Identity!.GetUserId(), resultOffset);

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}
