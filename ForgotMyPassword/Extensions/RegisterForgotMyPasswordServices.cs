namespace ForgotMyPassword.Extensions;

public static class RegisterForgotMyPasswordServices
{
    public static MauiAppBuilder ConfigureForgotMyPasswordServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ForgotMyPasswordViewModel>();
        
        
        return builder;
    }
}