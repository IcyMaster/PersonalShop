using PersonalShop.Interfaces.Commands;

namespace PersonalShop.Features.Carts.Commands;

public class DeleteProductFromCartCommand : IDeleteProductFromCartCommand
{
    public int ProductId { get; set; }
}
