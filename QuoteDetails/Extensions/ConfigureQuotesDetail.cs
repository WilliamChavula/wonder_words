using CommunityToolkit.Maui;
using QuoteDetails.ViewModels;
using QuoteDetails.Views;

namespace QuoteDetails.Extensions;

public static class ConfigureQuotesDetail
{
    public static MauiAppBuilder UseQuotesDetail(this MauiAppBuilder builder)
    {
        builder.UseMauiCommunityToolkit();
        builder.Services.AddSingleton<QuoteDetailsPage>();


        builder.Services.AddSingleton<QuoteDetailsViewModel>();

        return builder;
    }
}