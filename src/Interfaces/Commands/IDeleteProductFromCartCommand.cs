namespace PersonalShop.Interfaces.Commands
{
    public interface IDeleteProductFromCartCommand
    {
        int ProductId { get; set; }
    }
}