using CommunityToolkit.Maui;
using ProfileMenu.ViewModels;

namespace ProfileMenu.Extensions;

public static class ConfigureProfileMenu
{
    public static MauiAppBuilder UseProfileMenu(this MauiAppBuilder builder)
    {
        builder.UseMauiCommunityToolkit();
        builder.Services.AddSingleton<ProfileMenuViewModel>();

        return builder;
    }
}