namespace PersonalShop.BusinessLayer.Services.Carts.Interfaces
{
    public interface IDeleteProductFromCartCommand
    {
        int ProductId { get; set; }
    }
}