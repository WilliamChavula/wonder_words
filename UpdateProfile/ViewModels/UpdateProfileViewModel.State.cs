using CommunityToolkit.Mvvm.ComponentModel;
using FormFields.Inputs;
using Email = FormFields.Inputs.Email;

namespace UpdateProfile.ViewModels;

public partial class UpdateProfileViewModel
{
    [ObservableProperty] private bool _updateProfileInProgress;
    [ObservableProperty] private Email _email = new(string.Empty);
    [ObservableProperty] private Username _username = new(string.Empty);
    [ObservableProperty] private OptionalPassword _password = new(string.Empty, true);
    [ObservableProperty] private OptionalPasswordConfirmation _passwordConfirmation = new(string.Empty, true);

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsSubmissionInProgress))]
    private SubmissionStatus _submissionStatus = SubmissionStatus.Idle;

    

    public bool IsSubmissionInProgress => SubmissionStatus is SubmissionStatus.InProgress;
}

public enum SubmissionStatus
{
    Idle,
    InProgress,
    Success,
    Error
}