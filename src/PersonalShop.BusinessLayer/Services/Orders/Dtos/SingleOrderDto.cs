using PersonalShop.Domain.Entities.Orders;

namespace PersonalShop.BusinessLayer.Services.Orders.Dtos;

public class SingleOrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public OrderUserDto User { get; set; } = null!;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public DateTimeOffset OrderDate { get; set; } = DateTime.Now;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.NoStatus;
    public List<SingleOrderItemDto> OrderItems { get; set; } = new List<SingleOrderItemDto>();
}
