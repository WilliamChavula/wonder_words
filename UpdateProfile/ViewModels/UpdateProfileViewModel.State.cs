using CommunityToolkit.Mvvm.ComponentModel;
using FormFields.Inputs;
using Email = FormFields.Inputs.Email;

namespace UpdateProfile.ViewModels;

public partial class UpdateProfileViewModel
{
    [ObservableProperty] private bool _updateProfileInProgress;
    [ObservableProperty] private Email _email;
    [ObservableProperty] private Username _username;
    [ObservableProperty] private OptionalPassword _password = new(string.Empty, true);
    [ObservableProperty] private OptionalPasswordConfirmation _passwordConfirmation = new(string.Empty, true);

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsSubmissionInProgress))]
    private SubmissionStatus _submissionStatus = SubmissionStatus.Idle;

    

    public bool IsSubmissionInProgress => SubmissionStatus is SubmissionStatus.InProgress;
}

internal enum SubmissionStatus
{
    Idle,
    InProgress,
    Success,
    Error
}