using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Email = FormFields.Inputs.Email;

namespace ForgotMyPassword;

public partial class ForgotMyPasswordViewModel(UserRepository.UserRepository userRepository) : ObservableObject
{
    [ObservableProperty] private ForgotMyPasswordState _state = new();

    [RelayCommand]
    private void OnEmailChanged(string newValue)
    {
        var previousValue = State.Email;
        var shouldValidate = previousValue.IsNotValid;

        var shouldRegister = newValue == previousValue.Value && previousValue.IsAlreadyRegistered;

        State = State.CopyWith(
            shouldValidate
                ? new Email(newValue, false, IsAlreadyRegistered: shouldRegister)
                : new Email(newValue),
            null
        );
    }

    [RelayCommand]
    private void OnEmailUnfocused()
    {
        State = State.CopyWith(
            email: new Email(State.Email.Value),
            status: null
        );
    }

    [RelayCommand]
    private async Task OnSubmit()
    {
        var email = new Email(State.Email.Value, IsPure: false);
        State = State.CopyWith(
            email: email,
            status: email.IsValid ? SubmissionStatus.InProgress : null
        );

        if (email.IsValid)
        {
            try
            {
                await userRepository.RequestPasswordResetEmail(email.Value);
                
                State = State.CopyWith(
                    null,
                    status: SubmissionStatus.Success
                );
            }
            catch (Exception _)
            {
                State = State.CopyWith(
                    null,
                    status: SubmissionStatus.Error
                );
            }
        }
    }
}