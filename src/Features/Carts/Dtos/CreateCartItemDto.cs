using PersonalShop.Resources.Validation.Cart;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Features.Carts.Dtos;

public class CreateCartItemDto
{
    [Required(ErrorMessageResourceType = typeof(CartMessages)
        , ErrorMessageResourceName = nameof(CartMessages.ProductIdRequired))]
    public int ProductId { get; set; }

    [Required(ErrorMessageResourceType = typeof(CartMessages)
        , ErrorMessageResourceName = nameof(CartMessages.QuantityRequired))]
    [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(CartMessages)
        , ErrorMessageResourceName = nameof(CartMessages.QuantityValueRangeError))]
    public int Quantity { get; set; } = 0;
}
