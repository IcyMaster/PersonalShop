using MassTransit;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Carts.Consumers;

public class DeleteProductFromCarts : IConsumer<DeleteProductFromCartCommand>
{
    private readonly ICartService _cartService;

    public DeleteProductFromCarts(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Consume(ConsumeContext<DeleteProductFromCartCommand> context)
    {
        await _cartService.DeleteProductFromAllCartsAsync(context.Message);
    }
}
