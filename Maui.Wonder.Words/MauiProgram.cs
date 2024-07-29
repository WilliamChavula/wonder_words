using CommunityToolkit.Maui;
using ControlsLibrary.Extensions;
using ForgotMyPassword.Extensions;
using QuoteDetails.Extensions;
using Maui.Wonder.Words.Services;
using Microsoft.Extensions.Logging;
using QuoteList.Extensions;
using SignIn.Extensions;
using SignUp.Extensions;
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
            .UseControlsLibrary()
            .UseQuotesDetail()
            .UseQuotesList()
            .UseSignIn()
            .UseSignUp();

        builder.Services.AddSingleton<QuotesApi.QuotesApi>();
        builder.Services.AddSingleton<QuoteRepository.QuoteRepository>();
        builder.Services.AddSingleton<LocalStorage.LocalStorage>();

        builder.Services.AddSingleton<NavigationService>();

        builder.Services.AddSingleton<ForgotMyPassword.Interfaces.INavigationService>(
            x => x.GetService<NavigationService>()!
        );
        builder.Services.AddSingleton<ProfileMenu.Interfaces.INavigationService>(
            x => x.GetService<NavigationService>()!
        );

        builder.Services.AddSingleton<QuoteDetails.Interfaces.INavigationService>(
            x => x.GetService<NavigationService>()!
        );

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}