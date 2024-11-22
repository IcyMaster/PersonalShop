using PersonalShop.Resources.Validations.Product;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Features.Products.Dtos;

public class UpdateProductDto
{
    [MaxLength(20, ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.NameLengthError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.NameRequired))]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.DescriptionLengthError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.DescriptionRequired))]
    public string Description { get; set; } = string.Empty;

    [Range(0.0, (double)decimal.MaxValue
        , ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.PriceRangeError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.PriceRequired))]
    public decimal Price { get; set; } = decimal.Zero;
}
