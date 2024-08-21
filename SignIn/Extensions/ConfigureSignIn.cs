using SignIn.ViewModels;
using SignIn.Views;

namespace SignIn.Extensions;

public static class ConfigureSignIn
{
    public static MauiAppBuilder UseSignIn(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<SignInPage>();

        builder.Services.AddSingleton<SignInViewModel>();
        return builder;
    }
}
