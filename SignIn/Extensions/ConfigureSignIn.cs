using SignIn.ViewModels;
using SignIn.Views;

namespace SignIn.Extensions;

public static class ConfigureSignIn
{
    public static MauiAppBuilder UseSignIn(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<SignInViewModel>();
        builder.Services.AddSingleton<SignInView>();
        return builder;
    }
}