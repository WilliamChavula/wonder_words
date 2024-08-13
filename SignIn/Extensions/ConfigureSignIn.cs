using SignIn.Views;

namespace SignIn.Extensions;

public static class ConfigureSignIn
{
    public static MauiAppBuilder UseSignIn(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<SignInPage>();
        return builder;
    }
}