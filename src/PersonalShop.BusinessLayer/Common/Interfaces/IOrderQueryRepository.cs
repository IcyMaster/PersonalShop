using PersonalShop.BusinessLayer.Services.Orders.Dtos;
using PersonalShop.Domain.Entities.Responses;
using PersonalShop.Domain.Contracts;

namespace PersonalShop.BusinessLayer.Common.Interfaces;
public interface IOrderQueryRepository
{
    Task<PagedResult<SingleOrderDto>> GetAllOrdersAsync(string userId, PagedResultOffset resultOffset);
}