using PersonalShop.Domain.Card.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card;

public class Cart
{
    public Cart(string userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

    [Key]
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public decimal TotalPrice { get; private set; }

    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public void IncreaseTotalPrice(decimal price)
    {
        TotalPrice = TotalPrice + price;
    }
}
