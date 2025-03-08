using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Services.Interfaces;

public interface IOrderService
{
    Task<ServiceResult<string>> ChangeOrderStatusAsync(int orderId, ChangeOrderStatusDto changeOrderStatusDto);
    Task<ServiceResult<string>> CreateOrderAsync(string userId);
    Task<ServiceResult<PagedResult<SingleOrderDto>>> GetAllOrderAsync(string userId, PagedResultOffset resultOffset);
    Task<ServiceResult<PagedResult<SingleOrderDto>>> GetAllOrderAsync(PagedResultOffset resultOffset);
}