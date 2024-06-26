using FormFields.lib;

namespace FormFields.Inputs;

/// <summary>
/// Represents an optional password field.
///
/// Useful when the password can or can't be changed, such as in the update profile screen.
/// </summary>
/// <param name="Value"></param>
/// <param name="IsPure"></param>
public record OptionalPassword(string Value, bool IsPure)
    : FormZInput<string, OptionalPasswordValidationError?>(Value, IsPure)
{
    protected override OptionalPasswordValidationError? Validator(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;
        if (value.Length is > 5 and <= 120)
            return null;
        return OptionalPasswordValidationError.InValid;
    }
}

public enum OptionalPasswordValidationError
{
    InValid
}