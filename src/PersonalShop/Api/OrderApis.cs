using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Shared.Contracts;

namespace PersonalShop.Api;

public static class OrderApis
{
    public static void RegisterOrderApis(this WebApplication app)
    {
        app.MapGet("api/orders/changestatus/{orderId:int}", [Authorize(Roles = RolesContract.Admin)] async ([FromBody] ChangeOrderStatusDto changeOrderStatusDto, IOrderService orderService, HttpContext context, int orderId) =>
        {
            var serviceResult = await orderService.ChangeOrderStatusAsync(orderId, changeOrderStatusDto);

            if (serviceResult.IsSuccess)
            {
                return Results.Ok(ApiResult<string>.Success(serviceResult.Result!));
            }

            return Results.Ok(ApiResult<string>.Failed(serviceResult.Errors));
        });
    }
}
