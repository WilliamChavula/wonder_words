using LocalStorage.Models;

namespace QuoteRepository;

public class QuoteLocalStorage(LocalStorage.LocalStorage localStorage)
{
    public async Task UpsertQuoteListPage(QuoteListPageCm quoteListPage, bool favoritesOnly)
    {
        var realm = await (favoritesOnly
            ? localStorage.GetFavoriteQuoteListPageRealm
            : localStorage.GetQuoteListPageRealm);

        await realm.WriteAsync(() => { realm.Add(quoteListPage); });
    }

    public async Task ClearQuoteListPageList(bool favoritesOnly)
    {
        var realm = await (favoritesOnly
            ? localStorage.GetFavoriteQuoteListPageRealm
            : localStorage.GetQuoteListPageRealm);

        await realm.WriteAsync(() => { realm.RemoveAll(); });
    }

    public async Task Clear()
    {
        await Task.WhenAll([FavoriteRealmLambda(), QuoteListRealmLambda()]);
        return;

        async Task QuoteListRealmLambda()
        {
            var quoteListRealm = await localStorage.GetQuoteListPageRealm;
            await quoteListRealm.WriteAsync(() => { quoteListRealm.RemoveAll(); });
        }

        async Task FavoriteRealmLambda()
        {
            var favoriteRealm = await localStorage.GetFavoriteQuoteListPageRealm;
            await favoriteRealm.WriteAsync(() => { favoriteRealm.RemoveAll(); });
        }
    }

    public async Task<QuoteListPageCm?> GetQuoteListPage(int pageNumber, bool favoritesOnly)
    {
        var realm = await (favoritesOnly
            ? localStorage.GetFavoriteQuoteListPageRealm
            : localStorage.GetQuoteListPageRealm);

        return realm.All<QuoteListPageCm>().FirstOrDefault(quote => quote.PageNumber == pageNumber);
    }

    public async Task<QuoteCm?> GetQuote(int id)
    {
        var quoteListRealm = await localStorage.GetQuoteListPageRealm;
        var favoriteRealm = await localStorage.GetFavoriteQuoteListPageRealm;

        var quoteList = quoteListRealm.All<QuoteListPageCm>().ToList();
        var favoritesList = favoriteRealm.All<QuoteListPageCm>().ToList();

        var completeList = favoritesList
            .Union(quoteList)
            .SelectMany(page => page.QuotesList)
            .FirstOrDefault(quote => quote.Id == id);

        return completeList;
    }

    public async Task UpdateQuote(QuoteCm updatedQuote, bool shouldUpdateFavorites)
    {
        List<Task> tasks = [];
        var quoteListRealm = await localStorage.GetQuoteListPageRealm;
        var favoriteRealm = await localStorage.GetFavoriteQuoteListPageRealm;

        var pageList = quoteListRealm.All<QuoteListPageCm>();

        var outdatedPage = pageList.First(page => page.QuotesList.Any(quote => quote.Id == updatedQuote.Id));

        var quoteUpdateTask = quoteListRealm.WriteAsync(() =>
        {
            var outdatedList = outdatedPage
                .QuotesList
                .Select(quote => quote.Id == updatedQuote.Id ? updatedQuote : quote)
                .ToList();

            outdatedPage.QuotesList.Clear();
            outdatedPage.QuotesList.AddAll(outdatedList);

            // foreach (var item in outdatedList)
            // {
            //     outdatedPage.QuotesList.Add(item);
            // }
        });

        tasks.Add(quoteUpdateTask);

        if (shouldUpdateFavorites)
        {
            tasks.Add(favoriteRealm.WriteAsync(() =>
            {
                var outdatedList = outdatedPage
                    .QuotesList
                    .Select(quote => quote.Id == updatedQuote.Id ? updatedQuote : quote)
                    .ToList();

                outdatedPage.QuotesList.Clear();
                outdatedPage.QuotesList.AddAll(outdatedList);
            }));
        }

        await Task.WhenAll(tasks);
    }
}

public static class CollectionExtension
{
    public static void AddAll<T>(this ICollection<T> collection, ICollection<T> newCollection)
    {
        foreach (var item in newCollection)
        {
            collection.Add(item);
        }
    }
}