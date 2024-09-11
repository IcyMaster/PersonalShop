using PersonalShop.Domain.Card.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card;

public class Cart
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}
