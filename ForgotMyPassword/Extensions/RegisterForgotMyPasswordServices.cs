using CommunityToolkit.Maui;
using ForgotMyPassword.Control;
using ForgotMyPassword.ViewModels;

namespace ForgotMyPassword.Extensions;

public static class RegisterForgotMyPasswordServices
{
    public static MauiAppBuilder UseForgotMyPasswordServices(this MauiAppBuilder builder)
    {
        builder.UseMauiCommunityToolkit();
        builder.Services.AddSingleton<ForgotMyPasswordDialog>();

        builder.Services.AddSingleton<ForgotMyPasswordViewModel>();


        return builder;
    }
}