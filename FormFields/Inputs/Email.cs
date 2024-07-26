using System.Text.RegularExpressions;
using FormFields.lib;

namespace FormFields.Inputs;

public partial record Email(string Value, bool IsPure = true, bool IsAlreadyRegistered = false) : FormZInput<string, EmailValidationError?>(Value, IsPure), IInput
{
    public bool IsAlreadyRegistered { get; set; } = IsAlreadyRegistered;

    [GeneratedRegex("""
                    (?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)
                    *|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])
                    *")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|
                    \[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]
                    ?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|
                    \\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])
                    """)]
    private static partial Regex EmailRegex();

    protected override EmailValidationError? Validator(string value)
    {
        return string.IsNullOrEmpty(Value) ? EmailValidationError.Empty :
            IsAlreadyRegistered ? EmailValidationError.AlreadyRegistered :
            EmailRegex().IsMatch(value) ? null : EmailValidationError.InValid;
    }

    public bool IsInputValid => IsValid;
    public bool IsInputPure => IsPure;
}

public static class EmailExtension
{
    public static Email CopyWith(this Email email, string? emailValue, bool? newIsPure, bool? isAlreadyRegistered)
    {
        return new Email(emailValue ?? email.Value, newIsPure ?? email.IsPure)
        {
            IsAlreadyRegistered = isAlreadyRegistered ?? email.IsAlreadyRegistered
        };
    }
}
public enum EmailValidationError
{
    Empty,
    InValid,
    AlreadyRegistered
}