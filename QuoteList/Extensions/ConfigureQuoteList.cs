using QuoteList.ViewModels;
using QuoteList.Views;

namespace QuoteList.Extensions;

public static class ConfigureQuoteList
{
    public static MauiAppBuilder UseQuotesList(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<QuoteListViewModel>();
        builder.Services.AddTransient<QuoteListPage>();
        return builder;
    }
}