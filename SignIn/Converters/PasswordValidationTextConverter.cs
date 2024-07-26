using System.Globalization;
using FormFields.Inputs;
using L10n = SignIn.Resources.Resources;

namespace SignIn.Converters;

public class PasswordValidationTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var passwordError = (PasswordValidationError?)value;

        return passwordError switch
        {
            null => null,
            PasswordValidationError.Empty => L10n.passwordTextFieldEmptyErrorMessage,
            _ => L10n.passwordTextFieldInvalidErrorMessage
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}