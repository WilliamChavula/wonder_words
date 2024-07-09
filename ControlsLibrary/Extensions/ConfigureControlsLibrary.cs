using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using UraniumUI;

namespace ControlsLibrary.Extensions;

public static class ConfigureControlsLibrary
{
    public static MauiAppBuilder UseControlsLibrary(this MauiAppBuilder builder)
    {
        builder
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .UseUraniumUI()
            .UseUraniumUIMaterial();

        return builder;
    }

    public static void AddMaterialFontImage(IFontCollection fontCollection)
    {
        fontCollection.AddMaterialIconFonts();
    }
}