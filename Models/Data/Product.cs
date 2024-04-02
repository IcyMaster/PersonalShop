using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Personal_Shop.Models.Data;

public class Product
{
    public long Id { get; set; }
    public string CreatorName { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "مقدار فیلد نام از 20 کاراکتر بیشتر است")]
    [Required(ErrorMessage = "لطفا فیلد نام را تکمیل نمایید")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "مقدار فیلد توضیحات از 50 کاراکتر بیشتر است")]
    [Required(ErrorMessage = "لطفا فیلد توضیحات را تکمیل نمایید")]
    public string Description { get; set; } = string.Empty;

    [Range(0.0, (double)decimal.MaxValue, ErrorMessage = "مقدار فیلد قیمت نباید منفی باشد")]
    [Required(ErrorMessage = "لطفا فیلد قیمت را تکمیل نمایید")]
    public decimal Price { get; set; }
}
