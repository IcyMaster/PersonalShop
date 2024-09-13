using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card.Dtos;

public class SingleCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
    public decimal TotalPrice { get; set; }

    public List<CartItem> CartItems { get; set; } = null!;
}
