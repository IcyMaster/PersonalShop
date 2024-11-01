using MassTransit;
using PersonalShop.Features.Carts.Commands;
using PersonalShop.Interfaces.Features;

namespace PersonalShop.Features.Carts.Consumers
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
