using PersonalShop.BusinessLayer.Services.Carts.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Carts.Commands;

public class UpdateProductInCartsCommand : IUpdateProductInCartsCommand
{
    public int ProductId { get; set; }

    public decimal Price { get; set; } = decimal.Zero;
}
