using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using FormFields.Inputs;
using FormFields.lib;
using Email = FormFields.Inputs.Email;
using UserRepo = UserRepository.UserRepository;

namespace UpdateProfile.ViewModels;
public partial class UpdateProfileViewModel : ObservableObject, IQueryAttributable
{
    private readonly UserRepo _userRepository;
    private readonly Func<Task> _onUpdateProfileSuccess;
    public UpdateProfileViewModel(UserRepo userRepository, Func<Task> onUpdateProfileSuccess)
    {
        _userRepository = userRepository;
        _onUpdateProfileSuccess = onUpdateProfileSuccess;

        FetchUser();
    }
    
    [RelayCommand]
    private void OnEmailChanged(string newValue)
    {
        var previousEmail = Email;
        var shouldValidate = previousEmail.IsNotValid;
        Email = shouldValidate
            ? new Email(newValue, false)
            {
                IsAlreadyRegistered = newValue == previousEmail.Value && previousEmail.IsAlreadyRegistered
            }
            : new Email(newValue);
    }
    
    [RelayCommand]
    private void OnUsernameChanged(string newValue)
    {
        var previousUsername = Username;
        var shouldValidate = previousUsername.IsNotValid;
        Username = shouldValidate
            ? new Username(newValue, false)
            {
                IsAlreadyRegistered = newValue == previousUsername.Value && previousUsername.IsAlreadyRegistered
            }
            : new Username(newValue);
    }

    [RelayCommand]
    private void OnPasswordChanged(string newValue)
    {
        var previousPassword = Password;
        var shouldValidate = previousPassword.IsNotValid;
        Password = shouldValidate 
            ? new OptionalPassword(newValue, false) 
            : new OptionalPassword(newValue, true);
    }

    [RelayCommand]
    private void OnPasswordConfirmationChanged(string newValue)
    {
        var previousPasswordConfirmation = PasswordConfirmation;
        var shouldValidate = previousPasswordConfirmation.IsNotValid;
        PasswordConfirmation = shouldValidate 
            ? new OptionalPasswordConfirmation(newValue, false)
            {
                Password = Password
            } 
            : new OptionalPasswordConfirmation(newValue, true);
    }
    
    [RelayCommand]
    private void OnEmailUnfocused()
    {
        Email = new Email(Email.Value, IsPure: false, IsAlreadyRegistered: Email.IsAlreadyRegistered);
    }
    
    [RelayCommand]
    private void OnUsernameUnfocused()
    {
        Username = new Username(Username.Value) { IsAlreadyRegistered = Username.IsAlreadyRegistered };
    }
    
    [RelayCommand]
    private void OnPasswordUnfocused()
    {
        Password = new OptionalPassword(Password.Value, false);
    }
    
    [RelayCommand]
    private void OnPasswordConfirmationUnfocused()
    {
        PasswordConfirmation = new OptionalPasswordConfirmation(PasswordConfirmation.Value, false)
        {
            Password = Password
        };
    }
    
    [RelayCommand]
    private async Task OnSubmit()
    {
        var username = new Username(Username.Value, false) { IsAlreadyRegistered = Username.IsAlreadyRegistered };
        var email = new Email(Email.Value, false, Email.IsAlreadyRegistered);
        var password = new OptionalPassword(Password.Value, false);
        var passwordConfirmation = new OptionalPasswordConfirmation(PasswordConfirmation.Value, false)
        {
            Password = Password
        };

        var isFormValid = FormZ.Validate([
            username,
            email,
            password,
            passwordConfirmation
        ]);

        Username = username;
        Email = email;
        Password = password;
        PasswordConfirmation = passwordConfirmation;

        if (isFormValid)
        {
            try
            {
                await _userRepository.UpdateProfile(Username.Value, Email.Value, Password.Value);
                SubmissionStatus = SubmissionStatus.Success;
            }
            catch (Exception error)
            {
                SubmissionStatus = error is not UsernameAlreadyTakenException
                    and not EmailAlreadyRegisteredException
                    ? SubmissionStatus.Error
                    : SubmissionStatus.Idle;

                Username = error is UsernameAlreadyTakenException
                    ? new Username(Username.Value, false)
                    {
                        IsAlreadyRegistered = true
                    }
                    : Username;

                Email = error is EmailAlreadyRegisteredException
                    ? new Email(Email.Value, false, true)
                    : Email;
            }
        }
    }

    [RelayCommand]
    private async Task OnUpdateProfileSuccess() => await _onUpdateProfileSuccess();

    private async void FetchUser()
    {
        await foreach (var user in _userRepository.GetUser())
        {
            if(user is null) return;
            
            Username = new Username(user.Username);
            Email = new Email(user.Email);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Email = (query["Email"] as Email)!;
        Username = (query["Username"] as Username)!;
    }
}