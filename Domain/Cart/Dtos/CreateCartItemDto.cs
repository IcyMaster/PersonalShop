using PersonalShop.Domain.Products;
using PersonalShop.Resources;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Domain.Card.Dtos;

public class CreateCartItemDto
{
    [Required(ErrorMessageResourceType = typeof(CartMessages)
        , ErrorMessageResourceName = nameof(CartMessages.ProductIdRequired))]
    public long ProductId { get; set; }

    [Required(ErrorMessageResourceType = typeof(CartMessages)
        , ErrorMessageResourceName = nameof(CartMessages.QuanityRequired))]
    [Range(1,int.MaxValue, ErrorMessageResourceType = typeof(CartMessages)
        ,ErrorMessageResourceName = nameof(CartMessages.ValueRangeError))]
    public int Quanity { get; set; }
}
