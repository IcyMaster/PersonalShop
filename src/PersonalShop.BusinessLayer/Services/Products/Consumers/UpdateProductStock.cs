using MassTransit;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Products.Commands;

namespace PersonalShop.BusinessLayer.Services.Products.Consumers
{
    public class UpdateProductStock : IConsumer<UpdateProductStockCommand>
    {
        private readonly IProductService _productService;

        public UpdateProductStock(IProductService productService)
        {
            _productService = productService;
        }

        public async Task Consume(ConsumeContext<UpdateProductStockCommand> context)
        {
            await _productService.UpdateProductStockAsync(context.Message);
        }
    }
}
