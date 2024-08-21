using DomainModels.Delegates;
using ForgotMyPassword.ViewModels;
using FormFields.Inputs;
using Maui.Wonder.Words.Interfaces;
using SignIn.ViewModels;
using SignUp.ViewModels;
using UpdateProfile.ViewModels;
using FormFieldsEmail = FormFields.Inputs.Email;

namespace Maui.Wonder.Words.Services;

public static class RegisterScreensExtension
{
    public static MauiAppBuilder RegisterScreens(
        this MauiAppBuilder builder,
        INavigationService navigationService
    )
    {
        # region DefineDelegates
        Task OnUpdateProfileTap(string email, string username)
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "Email", new FormFieldsEmail(email) },
                { "Username", new Username(username) }
            };

            return navigationService.GoToAsync(
                $"{RouteConstants.UpdateProfileScreen}",
                navigationParameters
            );
        }
        Task OnSignInTap() => navigationService.GoToAsync($"{RouteConstants.SignInScreen}");
        Task OnSignUpTap() => navigationService.GoToAsync($"{RouteConstants.SignUpScreen}");
        Task OnAuthenticationError() =>
            navigationService.GoToAsync($"{RouteConstants.SignInScreen}");
        Task OnQuoteSelected(string quoteId) =>
            navigationService.GoToAsync($"quoteDetails?quoteId={quoteId}");
        Task OnForgotMyPasswordTap() =>
            navigationService.GoToAsync($"{RouteConstants.ForgotMyPasswordScreen}");
        Task OnCancelTap() => navigationService.GoBackAsync();
        Task OnEmailRequestSuccess() => navigationService.GoBackAsync();
        Task OnSignInSuccess() => navigationService.GoBackAsync();
        Task OnSignUpSuccess() => navigationService.GoBackAsync();
        Task OnUpdateProfileSuccess() => navigationService.GoBackAsync();

        #endregion

        #region RegisterServices

        builder.Services.AddSingleton<SignUpTapDelegate>(OnSignUpTap);
        builder.Services.AddSingleton<SignInTapDelegate>(OnSignInTap);
        builder.Services.AddSingleton<CancelTapDelegate>(OnCancelTap);
        builder.Services.AddSingleton<EmailRequestSuccessDelegate>(OnEmailRequestSuccess);
        builder.Services.AddSingleton<UpdateProfileTapDelegate>(OnUpdateProfileTap);
        builder.Services.AddSingleton<AuthenticationErrorDelegate>(OnAuthenticationError);
        builder.Services.AddSingleton<QuoteSelectedDelegate>(OnQuoteSelected);
        builder.Services.AddSingleton<SignInSuccessDelegate>(OnSignInSuccess);
        builder.Services.AddSingleton<ForgotMyPasswordTapDelegate>(OnForgotMyPasswordTap);
        builder.Services.AddSingleton<SignUpSuccessDelegate>(OnSignUpSuccess);
        builder.Services.AddSingleton<UpdateProfileSuccessDelegate>(OnUpdateProfileSuccess);
        builder.Services.AddSingleton<ForgotMyPasswordViewModel>();
        builder.Services.AddSingleton<SignInViewModel>();
        builder.Services.AddSingleton<SignUpViewModel>();
        builder.Services.AddSingleton<UpdateProfileViewModel>();

        #endregion

        return builder;
    }
}
