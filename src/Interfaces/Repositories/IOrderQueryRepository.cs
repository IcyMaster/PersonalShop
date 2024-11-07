using PersonalShop.Features.Orders.Dtos;

namespace PersonalShop.Interfaces.Repositories;
public interface IOrderQueryRepository
{
    Task<List<SingleOrderDto>> GetAllOrdersAsync(string userId);
}