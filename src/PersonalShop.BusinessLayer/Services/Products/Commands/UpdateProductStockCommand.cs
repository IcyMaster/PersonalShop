using PersonalShop.BusinessLayer.Services.Products.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Products.Commands;

public class UpdateProductStockCommand : IUpdateProductStockCommand
{
    public int ProductId { get; set; }
    public int Stock { get; set; } = 0;
}
