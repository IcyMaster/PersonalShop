using PersonalShop.BusinessLayer.Services.Carts.Interfaces;

namespace PersonalShop.BusinessLayer.Services.Carts.Commands;

public class DeleteProductFromCartCommand : IDeleteProductFromCartCommand
{
    public int ProductId { get; set; }
}
