using ForgotMyPassword.ViewModels;
using FormFields.Inputs;
using Maui.Wonder.Words.Interfaces;
using ProfileMenu.ViewModels;
using QuoteDetails.ViewModels;
using QuoteList.Delegates;
using QuoteList.ViewModels;
using QuoteList.Views;
using SignIn.ViewModels;
using SignUp.ViewModels;
using UpdateProfile.ViewModels;
using FormFieldsEmail = FormFields.Inputs.Email;

namespace Maui.Wonder.Words.Services;

public static class RegisterScreensExtension
{
    private delegate Task SignUpTapDelegate();

    private delegate Task CancelTapDelegate();

    private delegate Task EmailRequestSuccessDelegate();

    private delegate Task SignInTapDelegate();

    private delegate Task UpdateProfileTapDelegate(string email, string username);

    private delegate Task SignInSuccessDelegate();

    private delegate Task ForgotMyPasswordTapDelegate();

    private delegate Task SignUpSuccessDelegate();

    private delegate Task UpdateProfileSuccessDelegate();

    public static MauiAppBuilder RegisterScreens(this MauiAppBuilder builder, INavigationService navigationService)
    {
        #region RegisterDelegates

        Task OnUpdateProfileTap(string email, string username)
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "Email", new FormFieldsEmail(email) },
                { "Username", new Username(username) }
            };

            return navigationService.GoToAsync($"/{RouteConstants.UpdateProfileScreen}", navigationParameters);
        }
        Task OnSignInTap() => navigationService.GoToAsync($"/{RouteConstants.SignInScreen}");

        Task OnSignUpTap() => navigationService.GoToAsync($"{RouteConstants.SignUpScreen}");
        Task OnAuthenticationError() => navigationService.GoToAsync($"/{RouteConstants.SignInScreen}");
        Task OnQuoteSelected(string quoteId) => navigationService
            .GoToAsync($"/{RouteConstants.QuoteDetailsScreen}?QuoteId={quoteId}");
        Task OnForgotMyPasswordTap() => navigationService.GoToAsync($"/{RouteConstants.ForgotMyPasswordScreen}");

        Task OnCancelTap() => navigationService.GoBackAsync();
        Task OnEmailRequestSuccess() => navigationService.GoBackAsync();
        Task OnSignInSuccess() => navigationService.GoBackAsync();
        Task OnSignUpSuccess() => navigationService.GoBackAsync();
        Task OnUpdateProfileSuccess() => navigationService.GoBackAsync();

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

        #endregion

        builder.Services.AddSingleton<ForgotMyPasswordViewModel>();

        builder.Services.AddSingleton<ProfileMenuViewModel>();

        builder.Services.AddSingleton<QuoteDetailsViewModel>();

        // builder.Services.AddSingleton<QuoteListViewModel>();

        builder.Services.AddSingleton<SignInViewModel>();

        builder.Services.AddSingleton<SignUpViewModel>();

        builder.Services.AddSingleton<UpdateProfileViewModel>();

        return builder;
    }
}