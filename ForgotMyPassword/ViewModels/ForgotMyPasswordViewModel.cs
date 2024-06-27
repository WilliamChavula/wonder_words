using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;

using Email = FormFields.Inputs.Email;
using ForgotMyPassword.Interfaces;
using FormFields.Inputs;

namespace ForgotMyPassword;

public partial class ForgotMyPasswordViewModel : ObservableObject
{
    private readonly UserRepository.UserRepository _userRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private ForgotMyPasswordState _state = new();
    [ObservableProperty] private bool _isSubmissionInProgress;
    [ObservableProperty] private EmailValidationError? _emailError;
    [ObservableProperty] private bool _isEntryFieldEmptyError;
    [ObservableProperty] private bool _isSubmissionStatusError;

    public ForgotMyPasswordViewModel(UserRepository.UserRepository userRepository,
        INavigationService navigationService)
    {
        _userRepository = userRepository;
        _navigationService = navigationService;
        IsSubmissionInProgress = State.SubmissionStatus == SubmissionStatus.InProgress;
        EmailError = State.Email.IsNotValid ? State.Email.Error : null;
        IsEntryFieldEmptyError = EmailError == EmailValidationError.Empty;
        IsSubmissionStatusError = State.SubmissionStatus == SubmissionStatus.Error;
    }

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
            email: new Email(State.Email.Value, IsPure: false),
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
                await _userRepository.RequestPasswordResetEmail(email.Value);

                State = State.CopyWith(
                    null,
                    status: SubmissionStatus.Success
                );
            }
            catch
            {
                State = State.CopyWith(
                    null,
                    status: SubmissionStatus.Error
                );
            }
        }
    }

    [RelayCommand]
    private async Task OnCancelTap() => await _navigationService.GoBackAsync();

    [RelayCommand]
    private async Task OnEmailRequestSuccess() => await _navigationService.GoBackAsync();

    async partial void OnStateChanged(ForgotMyPasswordState value)
    {
        if (value.SubmissionStatus != SubmissionStatus.Success) return;
        
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.GhostWhite,
            TextColor = Colors.Black,
            CornerRadius = new CornerRadius(6),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14.0),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14.0),
            CharacterSpacing = 0.5
        };

        var message = Resources.Resources.emailRequestSuccessMessage;
        var duration = TimeSpan.FromSeconds(8);

        var snackbar = Snackbar.Make(message, duration: duration, visualOptions: snackbarOptions);
        await snackbar.Dismiss();
        await snackbar.Show();

        EmailRequestSuccessCommand.Execute(null);

    }
}