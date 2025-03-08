using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.Domain.Contracts;
using PersonalShop.Domain.Entities.Responses;

namespace PersonalShop.BusinessLayer.Common.Interfaces;
public interface IOrderQueryRepository
{
    Task<PagedResult<SingleOrderDto>> GetAllOrdersAsync(string userId, PagedResultOffset resultOffset);
    Task<PagedResult<SingleOrderDto>> GetAllOrdersAsync(PagedResultOffset resultOffset);
}