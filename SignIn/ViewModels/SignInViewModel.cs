using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using FormFields.Inputs;
using FormFields.lib;
using Email = FormFields.Inputs.Email;
using UserRepo = UserRepository.UserRepository;

namespace SignIn.ViewModels;

public partial class SignInViewModel(UserRepo userRepository) : ObservableObject
{
    [ObservableProperty] private Email _email = new(string.Empty);
    [ObservableProperty] private Password _password = new(string.Empty);
    [ObservableProperty] private SubmissionStatus _submissionStatus = SubmissionStatus.Idle;

    [RelayCommand]
    private void OnEmailChanged(string newValue)
    {
        var previousEmail = Email;
        var shouldValidate = previousEmail.IsNotValid;

        Email = shouldValidate ? new Email(newValue, IsPure: false) : new Email(newValue);
    }

    [RelayCommand]
    private void OnEmailUnfocused()
    {
        var previousEmail = Email;
        var previousEmailValue = previousEmail.Value;

        Email = new Email(previousEmailValue, false);
    }

    [RelayCommand]
    private void OnPasswordChanged(string newValue)
    {
        var previousPasswordState = Password;
        var shouldValidate = previousPasswordState.IsNotValid;
        Password = shouldValidate ? new Password(newValue, false) : new Password(newValue);
    }

    [RelayCommand]
    private void OnPasswordUnfocused()
    {
        var previousPasswordState = Password;
        var previousPasswordValue = previousPasswordState.Value;

        Password = new Password(previousPasswordValue, false);
    }

    [RelayCommand]
    private async Task OnSubmit()
    {
        var email = new Email(Email.Value, false);
        var password = new Password(Password.Value, false);

        var isFormValid = FormZ.Validate([email, password]); // email.IsValid && password.IsValid;

        if (isFormValid)
        {
            try
            {
                await userRepository.SignIn(email.Value, password.Value);
                SubmissionStatus = SubmissionStatus.Success;
                
                // Todo: Navigate to previous page
            }
            catch (Exception e)
            {
                SubmissionStatus = e is InvalidCredentialsException
                    ? SubmissionStatus.InvalidCredentialsError
                    : SubmissionStatus.GenericError;
            }
        }
    }

    [RelayCommand]
    private void OnSignUpTap()
    {
        // Todo: Navigate to previous page
    }

    [RelayCommand]
    private void OnForgotPasswordTap()
    {
    }
}

public enum SubmissionStatus
{
    // Used when the form has not been sent yet.
    Idle,

    // Used to disable all buttons and add a progress indicator to the main one.
    InProgress,

    // Used to close the screen and navigate back to the caller screen.
    Success,

    // Used to display a generic snack bar saying that an error has occurred, e.g., no internet connection.
    GenericError,

    // Used to show a more specific error telling the user they got the email and/or password wrong.
    InvalidCredentialsError
}