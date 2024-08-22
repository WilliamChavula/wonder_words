using System.Diagnostics;
using LocalStorage.Models;

namespace QuoteRepository;

public class QuoteLocalStorage(LocalStorage.LocalStorage localStorage)
{
    private readonly LocalStorage.LocalStorage localStorage = localStorage;

    public void UpsertQuoteListPage(QuoteListPageCm quoteListPage, bool favoritesOnly)
    {
        var realm = (
            favoritesOnly
                ? localStorage.GetFavoriteQuoteListPageRealm
                : localStorage.GetQuoteListPageRealm
        );

        realm.Write(() =>
        {
            realm.Add(quoteListPage);
        });
    }

    public void ClearQuoteListPageList(bool favoritesOnly)
    {
        var realm = favoritesOnly
            ? localStorage.GetFavoriteQuoteListPageRealm
            : localStorage.GetQuoteListPageRealm;

        realm.Write(() =>
        {
            realm.RemoveAll();
        });
    }

    public async Task Clear()
    {
        await Task.WhenAll([OnFavoriteRealmLambda(), OnQuoteListRealmLambda()]);
        return;

        Task OnQuoteListRealmLambda() =>
            Task.Run(() =>
            {
                var quoteListRealm = localStorage.GetQuoteListPageRealm;
                quoteListRealm.Write(() =>
                {
                    quoteListRealm.RemoveAll();
                });
            });

        Task OnFavoriteRealmLambda() =>
            Task.Run(() =>
            {
                var favoriteRealm = localStorage.GetFavoriteQuoteListPageRealm;
                favoriteRealm.WriteAsync(() =>
                {
                    favoriteRealm.RemoveAll();
                });
            });
    }

    public QuoteListPageCm? GetQuoteListPage(int pageNumber, bool favoritesOnly)
    {
        var realm = favoritesOnly
            ? localStorage.GetFavoriteQuoteListPageRealm
            : localStorage.GetQuoteListPageRealm;

        var query = realm.All<QuoteListPageCm>();

        if (!query.Any())
            return null;

        return query.FirstOrDefault(quote => quote.PageNumber == pageNumber);
    }

    public QuoteCm? GetQuote(int id)
    {
        var quoteListRealm = localStorage.GetQuoteListPageRealm;
        var favoriteRealm = localStorage.GetFavoriteQuoteListPageRealm;

        var query = quoteListRealm.All<QuoteListPageCm>();

        if (!query.Any())
            return null;

        var quoteList = query.ToList();
        var favoritesList = favoriteRealm.All<QuoteListPageCm>().ToList();

        var completeList = quoteList
            .SelectMany(page => page.QuotesList)
            .FirstOrDefault(quote => quote.Id == id);

        return completeList;
    }

    public void UpdateQuote(QuoteCm updatedQuote, bool shouldUpdateFavorites)
    {
        List<Task> tasks = [];
        var quoteListRealm = localStorage.GetQuoteListPageRealm;
        var favoriteRealm = localStorage.GetFavoriteQuoteListPageRealm;

        var pageList = quoteListRealm.All<QuoteListPageCm>();

        var outdatedPage = pageList.First(page =>
            page.QuotesList.Any(quote => quote.Id == updatedQuote.Id)
        );

        var quoteUpdateTask = quoteListRealm.WriteAsync(() =>
        {
            var outdatedList = outdatedPage
                .QuotesList.Select(quote => quote.Id == updatedQuote.Id ? updatedQuote : quote)
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
            tasks.Add(
                favoriteRealm.WriteAsync(() =>
                {
                    var outdatedList = outdatedPage
                        .QuotesList.Select(quote =>
                            quote.Id == updatedQuote.Id ? updatedQuote : quote
                        )
                        .ToList();

                    outdatedPage.QuotesList.Clear();
                    outdatedPage.QuotesList.AddAll(outdatedList);
                })
            );
        }

        Task.WhenAll(tasks);
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
