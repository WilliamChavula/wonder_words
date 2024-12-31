using System.Text.RegularExpressions;
using FormFields.lib;

namespace FormFields.Inputs;

public partial class Email(string Value, bool IsPure = true, bool IsAlreadyRegistered = false)
    : FormZInput<string, EmailValidationError?>(Value, IsPure),
        IInput
{
    public bool IsAlreadyRegistered { get; set; } = IsAlreadyRegistered;

    [GeneratedRegex(
        "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$"
    )]
    private static partial Regex EmailRegex();

    protected override EmailValidationError? Validator(string value)
    {
        var e = EmailRegex().IsMatch(value);
        if (string.IsNullOrEmpty(Value))
            return EmailValidationError.Empty;
        if (IsAlreadyRegistered)
            return EmailValidationError.AlreadyRegistered;
        if (EmailRegex().IsMatch(value))
            return null;

        return EmailValidationError.InValid;
    }

    public bool IsInputValid => IsValid;
    public bool IsInputPure => IsPure;
}

public static class EmailExtension
{
    public static Email CopyWith(
        this Email email,
        string? emailValue,
        bool? newIsPure,
        bool? isAlreadyRegistered
    )
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
