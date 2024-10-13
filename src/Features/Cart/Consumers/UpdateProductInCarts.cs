using MassTransit;
using PersonalShop.Domain.Carts.Commands;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Features.Cart.Consumers
{
    public class UpdateProductInCarts : IConsumer<UpdateProductInCartsCommand>
    {
        private readonly ICartService _cartService;

        public UpdateProductInCarts(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<UpdateProductInCartsCommand> context)
        {
            await _cartService.UpdateProductInCartsAsync(context.Message);
        }
    }
}
