using ForgotMyPassword.Control;

namespace ForgotMyPassword.Extensions;

public static class RegisterForgotMyPasswordServices
{
    public static MauiAppBuilder ConfigureForgotMyPasswordServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ForgotMyPasswordViewModel>();
        builder.Services.AddSingleton<ForgotMyPasswordDialog>();
        
        
        return builder;
    }
}