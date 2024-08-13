using CommunityToolkit.Mvvm.ComponentModel;
using FormFields.Inputs;
using Email = FormFields.Inputs.Email;

namespace SignUp.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    [ObservableProperty] private Email _email = new(string.Empty);
    [ObservableProperty] private Username _username = new(string.Empty);
    [ObservableProperty] private Password _password = new(string.Empty);
    [ObservableProperty] private PasswordConfirmation _passwordConfirmation = new(string.Empty, true);
    [ObservableProperty] private SubmissionStatus _submissionStatus = SubmissionStatus.Idle;
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
    Error,

    // Used to show a more specific error telling the user they got the email and/or password wrong.
    InvalidCredentialsError
}