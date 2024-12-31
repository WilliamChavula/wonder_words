using SignUp.ViewModels;
using SignUp.Views;

namespace SignUp.Extensions;

public static class ConfigureSignUp
{
    public static MauiAppBuilder UseSignUp(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<SignUpPage>();
        builder.Services.AddSingleton<SignUpViewModel>();
        return builder;
    }
}
