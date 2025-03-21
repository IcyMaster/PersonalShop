﻿using PersonalShop.Shared.Resources.Validations.Authentication;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;

public class RegisterDto
{
    [MaxLength(20, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.FirstNameMaxLengthError))
        , MinLength(3, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.FirstNameMinLengthError))]
    public string? FirstName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.LastNameMaxLengthError))
        , MinLength(3, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.LastNameMinLengthError))]
    public string? LastName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.UserNameMaxLengthError))
        , MinLength(3, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.UserNameMinLengthError))]
    [Required(ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.UserNameRequiredError))]
    public string UserName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.EmailValidateError))]
    [Required(ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.EmailRequiredError))]
    public string Email { get; set; } = string.Empty;

    [RegularExpression("^[0][9][0-9]{9,9}$", ErrorMessageResourceType = typeof(RegisterMessages)
        , ErrorMessageResourceName = nameof(RegisterMessages.PhoneNumberValidateError))]
    public string? PhoneNumber { get; set; } = string.Empty;

    [MinLength(6, ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.PasswordMinLengthError))]
    [Required(ErrorMessageResourceType = typeof(RegisterMessages), ErrorMessageResourceName = nameof(RegisterMessages.PasswordRequiredError))]
    public string Password { get; set; } = string.Empty;
}
