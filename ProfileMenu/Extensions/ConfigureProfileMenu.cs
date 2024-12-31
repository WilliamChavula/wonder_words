using CommunityToolkit.Maui;
using ProfileMenu.ViewModels;
using ProfileMenu.Views;

namespace ProfileMenu.Extensions;

public static class ConfigureProfileMenu
{
    public static MauiAppBuilder UseProfileMenu(this MauiAppBuilder builder)
    {
        // builder.UseMauiCommunityToolkit();
        builder.Services.AddSingleton<ProfileMenuPage>();
        builder.Services.AddSingleton<ProfileMenuViewModel>();

        return builder;
    }
}
