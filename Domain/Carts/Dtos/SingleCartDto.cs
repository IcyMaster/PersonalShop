using PersonalShop.Domain.Carts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public int TotalItemCount { get; private set; }
    public List<CartItemDto> CartItems { get; set; } = null!;

    public void SetTotalItemCount()
    {
        int total = 0;
        foreach (var item in CartItems)
        {
            total = total + item.Quanity;
        }

        TotalItemCount = total;
    }
}
