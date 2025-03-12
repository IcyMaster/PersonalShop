using MassTransit;
using PersonalShop.BusinessLayer.Services.Carts.Commands;
using PersonalShop.BusinessLayer.Services.Interfaces;
using PersonalShop.BusinessLayer.Services.Products.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
