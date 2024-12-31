using System.Globalization;
using FormFields.Inputs;
using L10n = SignIn.Resources.Resources;

namespace SignIn.Converters;

public class EmailValidationTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var emailError = (EmailValidationError?)value!;

        return emailError switch
        {
            null => null,
            EmailValidationError.Empty => L10n.emailTextFieldEmptyErrorMessage,
            _ => L10n.emailTextFieldInvalidErrorMessage
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}