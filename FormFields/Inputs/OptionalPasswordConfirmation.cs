using FormFields.lib;

namespace FormFields.Inputs;

public class OptionalPasswordConfirmation(string Value, bool IsPure)
    : FormZInput<string, OptionalPasswordConfirmationValidationError?>(Value, IsPure),
        IInput
{
    public OptionalPassword Password { get; set; } = new(string.Empty, true);

    protected override OptionalPasswordConfirmationValidationError? Validator(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            if (string.IsNullOrEmpty(Password.Value))
            {
                return null;
            }

            return OptionalPasswordConfirmationValidationError.Empty;
        }

        if (value == Password.Value)
            return null;

        return OptionalPasswordConfirmationValidationError.Invalid;
    }

    public bool IsInputValid => IsValid;
    public bool IsInputPure => IsPure;
}

public enum OptionalPasswordConfirmationValidationError
{
    Empty,
    Invalid,
}
