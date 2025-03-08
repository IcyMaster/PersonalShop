using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Controllers;

[Route("Orders")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Admin)]
    public async Task<ActionResult<PagedResult<SingleOrderDto>>> Index([FromQuery] PagedResultOffset resultOffset)
    {
        var serviceResult = await _orderService.GetAllOrderAsync(resultOffset);

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }

    [Authorize(Roles = RolesContract.Admin)]
    [HttpPost]
    [Route("ChangeStatus/{orderId:int}")]
    public async Task<ActionResult> ChangeStatus(int orderId, ChangeOrderStatusDto changeOrderStatusDto)
    {
        var serviceResult = await _orderService.ChangeOrderStatusAsync(orderId, changeOrderStatusDto);

        if (serviceResult.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}
