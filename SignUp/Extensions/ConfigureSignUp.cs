using SignUp.ViewModels;
using SignUp.Views;

namespace SignUp.Extensions;

public static class ConfigureSignUp
{
    public static MauiAppBuilder UseSignUp(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<SignUpViewModel>();
        builder.Services.AddTransient<SignUpView>();
        return builder;
    }
}