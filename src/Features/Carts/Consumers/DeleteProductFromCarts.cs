using MassTransit;
using PersonalShop.Features.Carts.Commands;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Features.Carts.Consumers;

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
