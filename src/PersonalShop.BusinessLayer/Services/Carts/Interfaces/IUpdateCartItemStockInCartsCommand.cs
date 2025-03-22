namespace PersonalShop.BusinessLayer.Services.Carts.Interfaces
{
    public interface IUpdateCartItemStockInCartsCommand
    {
        int ProductId { get; set; }
        int Stock { get; set; }
    }
}