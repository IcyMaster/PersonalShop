using PersonalShop.Data.Contracts;
using PersonalShop.Domain.Responses;
using PersonalShop.Features.Orders.Dtos;

namespace PersonalShop.Interfaces.Repositories;
public interface IOrderQueryRepository
{
    Task<PagedResult<SingleOrderDto>> GetAllOrdersAsync(string userId, PagedResultOffset resultOffset);
}