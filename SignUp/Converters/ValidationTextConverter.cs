using System.Globalization;
using FormFields.Inputs;
using L10n = SignUp.Resources.Resources;

namespace SignUp.Converters;

public class ValidationTextConverter<T> : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is EmailValidationError emailError)
        {
            return emailError switch
            {
                EmailValidationError.Empty => L10n.emailTextFieldEmptyErrorMessage,
                _ => L10n.emailTextFieldInvalidErrorMessage
            };
        }

        if (value is UsernameValidationError usernameValidationError)
        {
            return usernameValidationError switch
            {
                UsernameValidationError.Empty => L10n.usernameTextFieldEmptyErrorMessage,
                UsernameValidationError.AlreadyTaken
                    => L10n.usernameTextFieldAlreadyTakenErrorMessage,
                _ => L10n.usernameTextFieldInvalidErrorMessage
            };
        }

        if (value is PasswordValidationError passwordValidationError)
        {
            return passwordValidationError switch
            {
                PasswordValidationError.Empty => L10n.passwordTextFieldEmptyErrorMessage,
                _ => L10n.passwordTextFieldInvalidErrorMessage
            };
        }

        if (value is PasswordConfirmationValidationError confirmationValidationError)
        {
            return confirmationValidationError switch
            {
                PasswordConfirmationValidationError.Empty
                    => L10n.passwordConfirmationTextFieldEmptyErrorMessage,
                _ => L10n.passwordConfirmationTextFieldInvalidErrorMessage
            };
        }

        return string.Empty;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
