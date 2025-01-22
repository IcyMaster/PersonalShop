using MassTransit;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Carts.Consumers
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
            await _cartService.UpdateProductInAllCartsAsync(context.Message);
        }
    }
}
