using PersonalShop.Resources.Validations.Category;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Features.Categories.Dtos;

public class CreateCategoryDto
{
    public int ParentId { get; set; } = 0;

    [MaxLength(20, ErrorMessageResourceType = typeof(CategoryMessages)
    , ErrorMessageResourceName = nameof(CategoryMessages.NameLengthError))]
    [Required(ErrorMessageResourceType = typeof(CategoryMessages)
    , ErrorMessageResourceName = nameof(CategoryMessages.NameRequired))]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessageResourceType = typeof(CategoryMessages)
    , ErrorMessageResourceName = nameof(CategoryMessages.DescriptionLengthError))]
    [Required(ErrorMessageResourceType = typeof(CategoryMessages)
    , ErrorMessageResourceName = nameof(CategoryMessages.DescriptionRequired))]
    public string Description { get; set; } = string.Empty;
}
