using PersonalShop.Domain.Entities.Users;

namespace PersonalShop.Domain.Entities.Orders;

public class Order
{
    public Order(string userId, decimal totalPrice, OrderStatus orderStatus)
    {
        UserId = userId;
        TotalPrice = totalPrice;
        OrderStatus = orderStatus;
    }

    public int Id { get; set; }
    public string UserId { get; private set; } = string.Empty;
    public User User { get; set; } = null!;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public OrderStatus OrderStatus { get; private set; } = OrderStatus.NoStatus;
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public bool ChangeOrderStatus(string orderStatus)
    {
        OrderStatus status;
        if (Enum.TryParse(orderStatus, true, out status))
        {
            OrderStatus = status;
            return true;
        }
        else
        {
            return false;
        }
    }
}
