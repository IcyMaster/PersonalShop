using MassTransit;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Carts.Consumers;

public class UpdateCartItemStockInCarts : IConsumer<UpdateCartItemStockInCartsCommand>
{
    private readonly ICartService _cartService;

    public UpdateCartItemStockInCarts(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Consume(ConsumeContext<UpdateCartItemStockInCartsCommand> context)
    {
        await _cartService.UpdateCartItemStockInCartsAsync(context.Message);
    }
}
