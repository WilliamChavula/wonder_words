using ForgotMyPassword.Control;

namespace ForgotMyPassword.Extensions;

public static class RegisterForgotMyPasswordServices
{
    public static MauiAppBuilder UseForgotMyPasswordServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ForgotMyPasswordDialog>();
        
        
        return builder;
    }
}