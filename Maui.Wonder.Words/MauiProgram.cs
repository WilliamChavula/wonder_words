using CommunityToolkit.Maui;
using ForgotMyPassword.Extensions;
using ForgotMyPassword.Interfaces;
using Maui.Wonder.Words.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
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
            })
            .RegisterUserRepositoryServices()
            .ConfigureForgotMyPasswordServices();

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