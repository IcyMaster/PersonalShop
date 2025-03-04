using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PersonalShop.Shared.Validations;

public class RequiredElement : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not null)
        {
            var list = value as IList;

            if (list is not null)
            {
                if (list.Count > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
