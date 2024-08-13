namespace QuoteList.Views;

public class QuoteListShellV2 : Shell
{
    public QuoteListShellV2(IServiceProvider serviceProvider)
    {
        var quoteListPage = serviceProvider.GetRequiredService<QuoteListPage>();
        var pageContent = new ShellContent
        {
            ContentTemplate = new DataTemplate(() => quoteListPage),
            Title = "Home",
            Route = nameof(QuoteListPage)
        };
        
        Items.Add(pageContent);
    }
    
}