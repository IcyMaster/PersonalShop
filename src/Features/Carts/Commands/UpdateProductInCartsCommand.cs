using PersonalShop.Interfaces.Commands;

namespace PersonalShop.Features.Carts.Commands;

public class UpdateProductInCartsCommand : IUpdateProductInCartsCommand
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
}
