namespace PersonalShop.Features.Orders.Dtos;

public class SingleOrderItemDto
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; } = decimal.Zero;
    public int Quanity { get; set; } = 0;
}
