using PersonalShop.Domain.Carts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
    public decimal TotalPrice { get; set; }
    public int TotalItemCount
    {
        get
        {
            int total = 0;

            foreach (var item in CartItems)
            {
                total = total + item.Quanity;
            }

            return total;
        }
    }
    public List<CartItemDto> CartItems { get; set; } = null!;
}
