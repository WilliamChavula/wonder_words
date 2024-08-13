using SignUp.Views;

namespace SignUp.Extensions;

public static class ConfigureSignUp
{
    public static MauiAppBuilder UseSignUp(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<SignUpPage>();
        return builder;
    }
}