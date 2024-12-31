using CommunityToolkit.Mvvm.ComponentModel;
using Email = FormFields.Inputs.Email;

namespace ForgotMyPassword.ViewModels;

public partial class ForgotMyPasswordViewModel
{
    [NotifyPropertyChangedFor(nameof(EmailError))]
    [ObservableProperty] private Email _email = new(string.Empty);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSubmissionInProgress))]
    [NotifyPropertyChangedFor(nameof(IsSubmissionStatusError))]
    private SubmissionStatus _submissionStatus = SubmissionStatus.Idle;
};

// public static class ForgotMyPasswordStateExtension
// {
//     public static ForgotMyPasswordState CopyWith(this ForgotMyPasswordState state, Email? email, SubmissionStatus? status)
//     {
//         return new ForgotMyPasswordState
//         {
//             Email = email ?? state.Email,
//             SubmissionStatus = status ?? state.SubmissionStatus
//         };
//     }
// }

public enum SubmissionStatus
{
    Idle,
    InProgress,
    Success,
    Error,
}