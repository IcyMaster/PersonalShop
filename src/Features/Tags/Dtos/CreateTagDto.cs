﻿using PersonalShop.Resources.Validations.Category;
using PersonalShop.Resources.Validations.Tag;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Features.Tags.Dtos;

public class CreateTagDto
{
    [MaxLength(10, ErrorMessageResourceType = typeof(TagMessages)
, ErrorMessageResourceName = nameof(TagMessages.NameLengthError))]
    [Required(ErrorMessageResourceType = typeof(CategoryMessages)
, ErrorMessageResourceName = nameof(TagMessages.NameRequired))]
    public string Name { get; set; } = string.Empty;
}