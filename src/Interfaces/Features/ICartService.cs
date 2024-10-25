using PersonalShop.Domain.Card.Dtos;
using PersonalShop.Domain.Carts.Commands;
using PersonalShop.Domain.Carts.Dtos;
using PersonalShop.Domain.Response;

namespace PersonalShop.Interfaces.Features
{
    public interface ICartService
    {
        Task<ServiceResult<string>> AddCartItemByUserIdAsync(string userId, CreateCartItemDto createCartItemDto);
        Task<ServiceResult<string>> DeleteCartItemByUserIdAsync(string userId, int productId);
        Task<ServiceResult<bool>> DeleteProductByProductIdFromAllCartsAsync(DeleteProductFromCartCommand command);
        Task<ServiceResult<SingleCartDto>> GetCartByCartIdWithOutProductAsync(Guid cartId);
        Task<ServiceResult<SingleCartDto>> GetCartByUserIdWithProductAsync(string userId);
        Task<ServiceResult<string>> UpdateCartItemQuantityByUserIdAsync(string userId, int productId, UpdateCartItemDto updateCartItemDto);
        Task<ServiceResult<bool>> UpdateProductInCartsAsync(UpdateProductInCartsCommand command);
    }
}