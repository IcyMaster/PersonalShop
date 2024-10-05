using MassTransit;
using PersonalShop.Domain.Carts.Commands;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Features.Cart.Consumers;

public class DeleteProductFromCarts : IConsumer<DeleteProductFromCartCommand>
{
    private readonly ICartService _cartService;

    public DeleteProductFromCarts(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Consume(ConsumeContext<DeleteProductFromCartCommand> context)
    {
        await _cartService.DeleteProductByProductIdFromAllCartsAsync(context.Message);
    }
}
