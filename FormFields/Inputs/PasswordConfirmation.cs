using FormFields.lib;

namespace FormFields.Inputs;

public record PasswordConfirmation(string Value, bool IsPure)
    : FormZInput<string, PasswordConfirmationValidationError?>(Value, IsPure)
{
    public Password Password { get; set; } = new(string.Empty);

    protected override PasswordConfirmationValidationError? Validator(string value)
    {
        if (string.IsNullOrEmpty(value))
            return PasswordConfirmationValidationError.Empty;
        if (value == Password.Value)
            return null;
        return PasswordConfirmationValidationError.Invalid;
    }
}

public enum PasswordConfirmationValidationError
{
    Empty,
    Invalid,
}