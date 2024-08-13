using System.Globalization;
using FormFields.Inputs;
using L10n = UpdateProfile.Resources.Resources;

namespace UpdateProfile.Converters;

public class ValidationTextConverter<T> : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        
        var error = (T?)value;

        if (error is EmailValidationError emailError)
        {
            return emailError switch
            {
                EmailValidationError.Empty => L10n.emailTextFieldEmptyErrorMessage,
                _ => L10n.emailTextFieldInvalidErrorMessage
            };
            
        }

        if (error is UsernameValidationError usernameValidationError)
        {
            return usernameValidationError switch
            {
                UsernameValidationError.Empty => L10n.usernameTextFieldEmptyErrorMessage,
                UsernameValidationError.AlreadyTaken => L10n.usernameTextFieldAlreadyTakenErrorMessage,
                _ => L10n.usernameTextFieldInvalidErrorMessage
            };
        }

        if (error is OptionalPasswordValidationError)
        {
            return L10n.passwordTextFieldInvalidErrorMessage;
        }

        if (error is OptionalPasswordConfirmationValidationError)
        {
            return L10n.passwordConfirmationTextFieldInvalidErrorMessage;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}