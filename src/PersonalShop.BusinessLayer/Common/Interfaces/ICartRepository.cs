using PersonalShop.Domain.Entities.Carts;

namespace PersonalShop.BusinessLayer.Common.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<List<Cart>> GetAllCartsWithoutProductAsync(bool track = true);
        Task<Cart?> GetCartDetailsWithoutProductAsync(string userId, bool track = true);
    }
}