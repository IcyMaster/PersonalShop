using System.ComponentModel.DataAnnotations;

namespace Personal_Shop.Models.Data;

public class Login
{
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
    [Required(ErrorMessage = "لطفا فیلد ایمیل را تکمیل نمایید")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "لطفا فیلد رمز عبور را تکمیل نمایید")]
    public string Password { get; set; } = string.Empty;
}
