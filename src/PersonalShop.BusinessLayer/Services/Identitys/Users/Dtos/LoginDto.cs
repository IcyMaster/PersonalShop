using PersonalShop.Shared.Resources.Validations.Authentication;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.BusinessLayer.Services.Identitys.Users.Dtos;

public class LoginDto
{
    [EmailAddress(ErrorMessageResourceType = typeof(LoginMessages)
        , ErrorMessageResourceName = nameof(LoginMessages.EmailValidateError))]
    [Required(ErrorMessageResourceType = typeof(LoginMessages)
        , ErrorMessageResourceName = nameof(LoginMessages.EmailRequired))]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(LoginMessages)
        , ErrorMessageResourceName = nameof(LoginMessages.PasswordRequired))]
    public string Password { get; set; } = string.Empty;
}
