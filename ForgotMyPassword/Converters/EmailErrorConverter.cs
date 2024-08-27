using System.Globalization;
using FormFields.Inputs;

namespace ForgotMyPassword.Converters;

public class EmailErrorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var emailError = (EmailValidationError?)value;
        return emailError switch
        {
            null => false,
            EmailValidationError.Empty => true,
            EmailValidationError.InValid => true,
            EmailValidationError.AlreadyRegistered => true,
            _ => throw new NotSupportedException()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}