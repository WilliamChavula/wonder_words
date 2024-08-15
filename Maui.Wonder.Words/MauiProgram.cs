using CommunityToolkit.Maui;
using ControlsLibrary.Extensions;
using ForgotMyPassword.Extensions;
using QuoteDetails.Extensions;
using Maui.Wonder.Words.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuoteList.Extensions;
using SignIn.Extensions;
using SignUp.Extensions;
using UpdateProfile.Extensions;
using UserRepository.Extensions;
using System.Reflection;
using Maui.Wonder.Words.Extensions;

namespace Maui.Wonder.Words;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {

        var builder = MauiApp.CreateBuilder();

        builder
            .AddEnvironmentVariables()
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIconsRegular");
                fonts.AddFont("MaterialIconsRound-Regular.otf", "MaterialIconsRound");
                ConfigureControlsLibrary.AddMaterialFontImage(fonts);
            })
            .UseUserRepositoryServices();

        builder.Services.AddSingleton<QuotesApi.UserTokenSupplier>(provider =>
        {
            var userRepository = provider.GetRequiredService<UserRepository.Services.IUserSecureStorage>();

            return userRepository.GetUserToken;
        });

        builder.Services.AddSingleton<QuotesApi.QuotesApi>();
        builder.Services.AddSingleton<QuoteRepository.QuoteRepository>();
        builder.Services.AddSingleton<QuoteRepository.QuoteLocalStorage>();
        builder.Services.AddSingleton<LocalStorage.LocalStorage>();

        builder.Services.AddSingleton<NavigationService>();

        builder
            .RegisterScreens(NavigationService.GetInstance())
            .UseControlsLibrary()
            // .UseForgotMyPasswordServices()
            .UseQuotesList()
            .UseQuotesDetail();
        // .UseSignIn()
        // .UseSignUp()
        // .UseUpdateProfile();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}