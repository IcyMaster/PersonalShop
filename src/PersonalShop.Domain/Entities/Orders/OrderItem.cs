namespace PersonalShop.Domain.Entities.Orders;

public class OrderItem
{
    public OrderItem(int productId, string productName, decimal productPrice, int quanity)
    {
        ProductId = productId;
        ProductName = productName;
        ProductPrice = productPrice;
        Quanity = quanity;
    }

    public int Id { get; set; }
    public int OrderId { get; set; } = 0;
    public int ProductId { get; set; } = 0;
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; } = decimal.Zero;
    public int Quanity { get; set; } = 0;
}
