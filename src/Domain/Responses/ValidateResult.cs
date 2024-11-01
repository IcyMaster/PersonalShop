namespace PersonalShop.Domain.Responses;

public class ValidateResult
{
    public bool IsValid { get; }
    public List<string>? Errors { get; }

    public ValidateResult(bool isValid = true)
    {
        IsValid = isValid;
    }
    public ValidateResult(List<string> errors, bool isValid = false)
    {
        IsValid = isValid;
        Errors = errors;
    }
}
