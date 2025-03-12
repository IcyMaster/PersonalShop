namespace PersonalShop.BusinessLayer.Services.Products.Interfaces
{
    internal interface IUpdateProductStockCommand
    {
        int ProductId { get; set; }
        int Stock { get; set; }
    }
}