namespace PersonalShop.Domain.Orders.Dtos;

public class SingleOrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public OrderUserDto User { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<SingleOrderItemDto> OrderItems { get; set; } = null!;
}
