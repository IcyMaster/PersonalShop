using PersonalShop.Domain.Responses;
using PersonalShop.Features.Carts.Commands;
using PersonalShop.Features.Carts.Dtos;

namespace PersonalShop.Interfaces.Features
{
    public interface ICartService
    {
        Task<ServiceResult<SingleCartDto>> GetCartDetailsWithCartItemsAsync(string userId);
        Task<ServiceResult<string>> AddCartItemAsync(string userId, CreateCartItemDto createCartItemDto);
        Task<ServiceResult<string>> DeleteCartItemAsync(string userId, int productId);
        Task<ServiceResult<string>> UpdateCartItemQuantityAsync(string userId, int productId, UpdateCartItemDto updateCartItemDto);
        Task<ServiceResult<bool>> DeleteProductFromAllCartsAsync(DeleteProductFromCartCommand command);
        Task<ServiceResult<bool>> UpdateProductInAllCartsAsync(UpdateProductInCartsCommand command);
    }
}