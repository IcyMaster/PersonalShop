using Microsoft.AspNetCore.Http;
using PersonalShop.Shared.Resources.Validations.Product;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.BusinessLayer.Services.Products.Dtos;

public class UpdateProductDto
{
    [MaxLength(20, ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.NameLengthError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.NameRequired))]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.DescriptionLengthError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.DescriptionRequired))]
    public string Description { get; set; } = string.Empty;

    [MaxLength(150, ErrorMessageResourceType = typeof(ProductMessages)
    , ErrorMessageResourceName = nameof(ProductMessages.ShortDescriptionLengthError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
    , ErrorMessageResourceName = nameof(ProductMessages.ShortDescriptionRequired))]
    public string ShortDescription { get; set; } = string.Empty;

    [Range(0.0, (double)decimal.MaxValue
        , ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.PriceRangeError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.PriceRequired))]
    public decimal Price { get; set; } = decimal.Zero;

    [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.StockRangeError))]
    [Required(ErrorMessageResourceType = typeof(ProductMessages)
        , ErrorMessageResourceName = nameof(ProductMessages.StockRequired))]
    public int Stock { get; set; } = 0;

    public IFormFile? Image { get; set; } = null!;

    public List<int>? Categories { get; set; }
    public List<int>? Tags { get; set; }
}
