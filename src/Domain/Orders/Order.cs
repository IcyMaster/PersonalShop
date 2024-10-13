using PersonalShop.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Orders;

public class Order
{
    public Order(string userId,decimal totalPrice)
    {
        UserId = userId;
        TotalPrice = totalPrice;
        OrderItems = new();
    }

    [Key]
    public int Id { get; set; }
    public string UserId { get; private set; } = string.Empty;
    public User User { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; }
}
