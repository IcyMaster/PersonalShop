namespace PersonalShop.Interfaces.Commands
{
    public interface IUpdateProductInCartsCommand
    {
        decimal Price { get; set; }
        int ProductId { get; set; }
    }
}