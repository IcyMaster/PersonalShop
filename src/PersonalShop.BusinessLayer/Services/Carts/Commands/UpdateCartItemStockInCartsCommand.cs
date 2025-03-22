using PersonalShop.BusinessLayer.Services.Carts.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Carts.Commands;

public class UpdateCartItemStockInCartsCommand : IUpdateCartItemStockInCartsCommand
{
    public int ProductId { get; set; }
    public int Stock { get; set; } = 0;
}
