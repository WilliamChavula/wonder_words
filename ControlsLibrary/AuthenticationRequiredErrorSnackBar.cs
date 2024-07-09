using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ControlsLibrary;

public static class AuthenticationRequiredErrorSnackBar
{
    public static async void MakeSnackBar()
    {
        var snackbarOptions = new SnackbarOptions
        {
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
        };

        var text = Resources.Resources.authenticationRequiredErrorSnackbarMessage;
        var duration = TimeSpan.FromSeconds(8);

        var snackbar = Snackbar.Make(text, duration: duration, visualOptions: snackbarOptions);
        await snackbar.Dismiss();
        await snackbar.Show();

    }
}