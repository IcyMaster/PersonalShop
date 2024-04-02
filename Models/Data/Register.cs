using System.ComponentModel.DataAnnotations;

namespace Personal_Shop.Models.Data;

public class Register
{
    [MaxLength(20,ErrorMessage = "مقدار فیلد نام از 20 کاراکتر بیشتر است"), MinLength(3, ErrorMessage = "مقدار فیلد نام از 3 کاراکتر کمتر است")]
    public string? FirstName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "مقدار فیلد نام  خانوادگی از 20 کاراکتر بیشتر است"), MinLength(3, ErrorMessage = "مقدار فیلد نام خانوادگی از 3 کاراکتر کمتر است")]
    public string? LastName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "مقدار فیلد نام  کاربری از 20 کاراکتر بیشتر است"), MinLength(3, ErrorMessage = "مقدار فیلد نام  کاربری از 3 کاراکتر کمتر است")]
    [Required(ErrorMessage = "لطفا فیلد نام کاربری را تکمیل نمایید")]
    public string UserName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
    [Required(ErrorMessage = "لطفا فیلد ایمیل را تکمیل نمایید")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression("/((0?9)|(\\+?989))\\d{9}/g", ErrorMessage = "شماره تلفن وارد شده معتبر نیست")]
    public string? PhoneNumber { get; set; } = string.Empty;

    [MinLength(6,ErrorMessage = "پسورد وارد شده باید بالای 6 کاراکتر باشد")]
    [Required(ErrorMessage = "لطفا فیلد رمز عبور را تکمیل نمایید")]
    public string Password { get; set; } = string.Empty;
}
