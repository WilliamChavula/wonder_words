using CommunityToolkit.Mvvm.ComponentModel;

namespace ForgotMyPassword;
using FormFields.Inputs;

public partial class ForgotMyPasswordState : ObservableObject
{
    [ObservableProperty]
    private Email _email  = new(string.Empty);
    
    [ObservableProperty]
    private SubmissionStatus _submissionStatus  = SubmissionStatus.Idle;
    
};

public static class ForgotMyPasswordStateExtension
{
    public static ForgotMyPasswordState CopyWith(this ForgotMyPasswordState state, Email? email, SubmissionStatus? status)
    {
        return new ForgotMyPasswordState
        {
            Email = email ?? state.Email,
            SubmissionStatus = status ?? state.SubmissionStatus
        };
    }
}

public enum SubmissionStatus {
    Idle,
    InProgress,
    Success,
    Error,
}