using FormFields.lib;

namespace FormFields.Inputs;

public class Password(string Value = "", bool IsPure = true)
    : FormZInput<string, PasswordValidationError?>(Value, IsPure),
        IInput
{
    protected override PasswordValidationError? Validator(string value)
    {
        if (string.IsNullOrEmpty(value))
            return PasswordValidationError.Empty;
        if (value.Length is < 5 or > 120)
            return PasswordValidationError.Invalid;

        return null;
    }

    public bool IsInputValid => IsValid;
    public bool IsInputPure => IsPure;
}

public enum PasswordValidationError
{
    Empty,
    Invalid,
}
