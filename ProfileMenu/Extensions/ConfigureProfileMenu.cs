using CommunityToolkit.Maui;
using ProfileMenu.Views;

namespace ProfileMenu.Extensions;

public static class ConfigureProfileMenu
{
    public static MauiAppBuilder UseProfileMenu(this MauiAppBuilder builder)
    {
        builder.UseMauiCommunityToolkit();
        builder.Services.AddSingleton<ProfileMenuPage>();

        return builder;
    }
}