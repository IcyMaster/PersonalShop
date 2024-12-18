﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Extension;
using PersonalShop.Features.Orders.Dtos;
using PersonalShop.Features.Products.Dtos;
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

    [HttpGet]
    [Route("Products", Name = "UserProducts")]
    [Authorize(Roles = RolesContract.Admin)]
    public async Task<ActionResult<PagedResult<SingleProductDto>>> UserProducts([FromQuery] PagedResultOffset resultOffset)
    {
        var validateObject = Extension.ObjectValidatorExtension.Validate(resultOffset);
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
