using Humanizer.Localisation;
using Newtonsoft.Json.Linq;
using Personal_Shop.Domain.Users;
using Personal_Shop.Resources;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Personal_Shop.Domain.Products.DTO;

public class ProductDTO
{
    [Key]
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public CustomUser? User { get; set; }

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
    [DisplayFormat(DataFormatString = "{0:G29}", ApplyFormatInEditMode = true)]
    public decimal Price { get; set; }
}
