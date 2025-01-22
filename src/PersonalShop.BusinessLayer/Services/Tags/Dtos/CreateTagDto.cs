using PersonalShop.Shared.Resources.Validations.Category;
using PersonalShop.Shared.Resources.Validations.Tag;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.BusinessLayer.Services.Tags.Dtos;

public class CreateTagDto
{
    [MaxLength(10, ErrorMessageResourceType = typeof(TagMessages)
, ErrorMessageResourceName = nameof(TagMessages.NameLengthError))]
    [Required(ErrorMessageResourceType = typeof(CategoryMessages)
, ErrorMessageResourceName = nameof(TagMessages.NameRequired))]
    public string Name { get; set; } = string.Empty;
}
