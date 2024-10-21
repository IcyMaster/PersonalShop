using PersonalShop.Domain.Response;
using PersonalShop.Domain.Users.Dtos;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Extension;

public static class ObjectValidator
{
    public static ValidateResult Validate(object instance)
    {
        var validateRes = new List<ValidationResult>();
        if (!Validator.TryValidateObject(instance, new ValidationContext(instance), validateRes, true))
        {
            return new ValidateResult(validateRes.Select(x => x.ErrorMessage).ToList()!);
        }
        else
        {
            return new ValidateResult(true);
        }
    }
}
