using UpdateProfile.Views;

namespace UpdateProfile.Extensions;

public static class ConfigureUpdateProfile
{
    public static MauiAppBuilder UseUpdateProfile(this MauiAppBuilder builder)
    {
        
        builder.Services.AddTransient<UpdateProfilePage>();
        
        return builder;
    }
}