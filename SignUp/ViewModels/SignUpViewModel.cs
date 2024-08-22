using CommunityToolkit.Mvvm.Input;
using DomainModels;
using DomainModels.Delegates;
using FormFields.Inputs;
using FormFields.lib;
using Email = FormFields.Inputs.Email;
using UserRepo = UserRepository.UserRepository;

namespace SignUp.ViewModels;

public partial class SignUpViewModel(UserRepo userRepository, SignUpSuccessDelegate onSignUpSuccess)
{
    [RelayCommand]
    private void OnEmailChanged(string newValue)
    {
        var previousEmail = Email;
        var shouldValidate = previousEmail.IsNotValid;
        Email = shouldValidate
            ? new Email(newValue, false, previousEmail.Value == newValue)
            : new Email(newValue);
    }

    [RelayCommand]
    private void OnEmailUnfocused()
    {
        Email = new Email(
            Email.Value,
            IsPure: false,
            IsAlreadyRegistered: Email.IsAlreadyRegistered
        );
        EmailError = Email.IsInputValid ? null : Email.DisplayError;
    }

    [RelayCommand]
    private void OnUsernameChanged(string newValue)
    {
        var previousUsername = Username;
        var shouldValidate = previousUsername.IsNotValid;
        Username = shouldValidate
            ? new Username(newValue, false)
            {
                IsAlreadyRegistered =
                    newValue == previousUsername.Value && previousUsername.IsAlreadyRegistered
            }
            : new Username(newValue);
    }

    [RelayCommand]
    private void OnUsernameUnfocused()
    {
        Username = new Username(Username.Value)
        {
            IsAlreadyRegistered = Username.IsAlreadyRegistered
        };
        UsernameError = Username.IsInputValid ? null : Username.DisplayError;
    }

    [RelayCommand]
    private void OnPasswordChanged(string newValue)
    {
        var previousPassword = Password;
        var shouldValidate = previousPassword.IsNotValid;
        Password = shouldValidate ? new Password(newValue, false) : new Password(newValue);
    }

    [RelayCommand]
    private void OnPasswordUnfocused()
    {
        Password = new Password(Password.Value, false);
        PasswordError = Password.IsInputValid ? null : Password.DisplayError;
    }

    [RelayCommand]
    private void OnPasswordConfirmationChanged(string newValue)
    {
        var previousPasswordConfirmation = PasswordConfirmation;
        var shouldValidate = previousPasswordConfirmation.IsNotValid;
        PasswordConfirmation = shouldValidate
            ? new PasswordConfirmation(newValue, false) { Password = Password }
            : new PasswordConfirmation(newValue, true);
    }

    [RelayCommand]
    private void OnPasswordConfirmationUnfocused()
    {
        PasswordConfirmation = new PasswordConfirmation(PasswordConfirmation.Value, false)
        {
            Password = Password
        };
        PasswordConfirmationError = PasswordConfirmation.IsInputValid
            ? null
            : PasswordConfirmation.DisplayError;
    }

    [RelayCommand]
    private async Task OnSubmit()
    {
        // Clear any errors that are being displayed
        PasswordError = null;
        EmailError = null;
        UsernameError = null;
        PasswordConfirmationError = null;

        var username = new Username(Username.Value, false)
        {
            IsAlreadyRegistered = Username.IsAlreadyRegistered
        };
        var email = new Email(Email.Value, false, Email.IsAlreadyRegistered);
        var password = new Password(Password.Value, false);
        var passwordConfirmation = new PasswordConfirmation(PasswordConfirmation.Value, false)
        {
            Password = Password
        };

        var isFormValid = FormZ.Validate([username, email, password, passwordConfirmation]);

        Username = username;
        Email = email;
        Password = password;
        PasswordConfirmation = passwordConfirmation;

        if (isFormValid)
        {
            try
            {
                await userRepository.SignUp(Username.Value, Email.Value, Password.Value);
                SubmissionStatus = SubmissionStatus.Success;
            }
            catch (Exception error)
            {
                SubmissionStatus = error
                    is not UsernameAlreadyTakenException
                        and not EmailAlreadyRegisteredException
                    ? SubmissionStatus.Error
                    : SubmissionStatus.Idle;

                Username =
                    error is UsernameAlreadyTakenException
                        ? new Username(Username.Value, false) { IsAlreadyRegistered = true }
                        : Username;

                Email =
                    error is EmailAlreadyRegisteredException
                        ? new Email(Email.Value, false, true)
                        : Email;
            }
        }
        else
        {
            PasswordError = password.DisplayError;
            EmailError = email.DisplayError;
            UsernameError = username.DisplayError;
            PasswordConfirmationError = passwordConfirmation.DisplayError;
        }
    }

    [RelayCommand]
    private async Task OnSignUpSuccess() => await onSignUpSuccess();
}
