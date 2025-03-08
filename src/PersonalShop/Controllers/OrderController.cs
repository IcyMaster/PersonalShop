using Microsoft.AspNetCore.Mvc;

namespace PersonalShop.Controllers;

[Route("Orders")]
public class OrderController : Controller
{
    public OrderController()
    {

    }

    [HttpGet]
    [Authorize(Roles = RolesContract.Customer)]
    public async Task<ActionResult<PagedResult<SingleOrderDto>>> Index([FromQuery] PagedResultOffset resultOffset)
    {
        var serviceResult = await _orderService.GetAllOrderAsync(User.Identity!.GetUserId(), resultOffset);

        if (serviceResult.IsSuccess)
        {
            return View(serviceResult.Result);
        }

        return BadRequest(ApiResult<string>.Failed(serviceResult.Errors));
    }
}
