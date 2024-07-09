using CommunityToolkit.Maui;
using ControlsLibrary.Extensions;
using ForgotMyPassword.Extensions;
using ForgotMyPassword.Interfaces;
using Maui.Wonder.Words.Services;
using Microsoft.Extensions.Logging;
using UserRepository.Extensions;

namespace Maui.Wonder.Words;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                ConfigureControlsLibrary.AddMaterialFontImage(fonts);
            })
            .RegisterUserRepositoryServices()
            .ConfigureForgotMyPasswordServices()
            .UseControlsLibrary();

        builder.Services.AddSingleton<QuotesApi.QuotesApi>();
        builder.Services.AddSingleton<QuoteRepository.QuoteRepository>();
        builder.Services.AddSingleton<LocalStorage.LocalStorage>();

        builder.Services.AddSingleton<INavigationService, NavigationService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}