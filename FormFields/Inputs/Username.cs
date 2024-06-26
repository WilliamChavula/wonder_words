using System.Text.RegularExpressions;
using FormFields.lib;

namespace FormFields.Inputs;

public partial record Username(string Value, bool IsPure = true)
    : FormZInput<string, UsernameValidationError?>(Value, IsPure)
{
    [GeneratedRegex("^(?=.{1,20}$)(?![_])(?!.*[_.]{2})[a-zA-Z0-9_]+(?<![_])$")]
    private static partial Regex UsernameRegex();

    public bool IsAlreadyRegistered { get; set; } = false;

    protected override UsernameValidationError? Validator(string value)
    {
        return string.IsNullOrEmpty(value) ? UsernameValidationError.Empty :
            IsAlreadyRegistered ? UsernameValidationError.AlreadyTaken :
            UsernameRegex().IsMatch(value) ? null : UsernameValidationError.Invalid;
    }
}

public enum UsernameValidationError
{
    Empty,
    Invalid,
    AlreadyTaken,
}