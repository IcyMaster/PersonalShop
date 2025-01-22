namespace PersonalShop.BusinessLayer.Services.Carts.Interfaces
{
    public interface IUpdateProductInCartsCommand
    {
        decimal Price { get; set; }
        int ProductId { get; set; }
    }
}