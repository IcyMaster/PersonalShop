using PersonalShop.Domain.Users;

namespace PersonalShop.Domain.Orders;

public class Order
{
    public Order(string userId, decimal totalPrice)
    {
        UserId = userId;
        TotalPrice = totalPrice;
    }

    public int Id { get; set; }
    public string UserId { get; private set; } = string.Empty;
    public User User { get; set; } = null!;
    public decimal TotalPrice { get; set; } = decimal.Zero;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
