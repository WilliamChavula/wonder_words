using UpdateProfile.ViewModels;

namespace UpdateProfile.Extensions;

public static class ConfigureUpdateProfile
{
    public static MauiAppBuilder UseUpdateProfile(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<UpdateProfileViewModel>();
        return builder;
    }
}