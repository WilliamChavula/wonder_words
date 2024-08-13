using DomainModels;
using QuoteRepository.Mappers;
using QuotesApi.Models;
using QuotesApi.Models.Response;

namespace QuoteRepository;

public class QuoteRepository(QuotesApi.QuotesApi quotesApi)
{
    private readonly QuoteLocalStorage _quoteLocalStorage =
        new(localStorage: new LocalStorage.LocalStorage());

    public async IAsyncEnumerable<QuoteListPage> GetQuoteListPage(
        int pageNumber,
        QuoteListPageFetchPolicy fetchPolicy,
        Tag? tag,
        string? favoredByUsername,
        string searchTerm = "")
    {
        var isFilteringByTag = tag is not null;
        var isSearching = !string.IsNullOrEmpty(searchTerm);
        var isFetchPolicyNetworkOnly = fetchPolicy == QuoteListPageFetchPolicy.NetworkOnly;
        var shouldSkipCacheLookup = isFilteringByTag || isSearching || isFetchPolicyNetworkOnly;

        if (shouldSkipCacheLookup)
        {
            var freshPage = await GetQuoteListPageFromNetwork(
                pageNumber,
                tag: tag,
                favoredByUsername: favoredByUsername,
                searchTerm: searchTerm);

            yield return freshPage;
        }
        else
        {
            var isFilteringByFavorites = favoredByUsername is not null;
            var cachedPage = _quoteLocalStorage.GetQuoteListPage(pageNumber, isFilteringByFavorites);

            var isFetchPolicyCacheAndNetwork = fetchPolicy == QuoteListPageFetchPolicy.CacheAndNetwork;
            var isFetchPolicyCachePreferably = fetchPolicy == QuoteListPageFetchPolicy.CachePreferably;
            var shouldEmitCachedPageInAdvance = isFetchPolicyCachePreferably || isFetchPolicyCacheAndNetwork;

            if (shouldEmitCachedPageInAdvance && cachedPage is not null)
            {
                yield return cachedPage.ToDomainModel();
            }

            var quoteListPage = await GetQuoteListPageFromNetwork(
                pageNumber,
                favoredByUsername: favoredByUsername,
                tag: null);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (_quoteLocalStorage is not null)
            {
                yield return quoteListPage;
            }
            else
            {
                var isFetchPolicyNetworkPreferably = fetchPolicy == QuoteListPageFetchPolicy.NetworkPreferably;
                if (cachedPage is not null && isFetchPolicyNetworkPreferably)
                {
                    yield return cachedPage.ToDomainModel();
                }
            }
        }
    }

    private async Task<QuoteListPage> GetQuoteListPageFromNetwork(
        int pageNumber,
        Tag? tag,
        string? favoredByUsername,
        string searchTerm = "")
    {
        QuoteListPageRm? quoteListPage;
        try
        {
            quoteListPage = await quotesApi.GetQuoteListPage(
                page: pageNumber,
                tag: tag?.ToApiModel(),
                favoredByUsername: favoredByUsername,
                searchTerm: searchTerm);

            var isFiltering = tag != null || !string.IsNullOrEmpty(searchTerm);
            var favoritesOnly = favoredByUsername is not null;
            var shouldStoreOnCache = !isFiltering;

            if (shouldStoreOnCache)
            {
                var shouldEmptyCache = pageNumber == 1;
                if (shouldEmptyCache)
                    _quoteLocalStorage.ClearQuoteListPageList(favoritesOnly);
            
                var quoteListCachePage = quoteListPage.ToCacheModel();
                _quoteLocalStorage.UpsertQuoteListPage(quoteListCachePage, favoritesOnly);
            }
        }
        catch (Exception ex) when (ex is EmptySearchResultQuoteException)
        {
            throw new EmptySearchResultException();
        }

        return quoteListPage.ToDomainModel();
    }

    public async Task<Quote> GetQuoteDetails(int id)
    {
        var cachedQuote = _quoteLocalStorage.GetQuote(id);
        if (cachedQuote is not null)
            return cachedQuote.ToDomainModel();

        var apiQuote = await quotesApi.GetQuote(id);

        return apiQuote.ToDomainModel();
    }

    public async Task<Quote> FavoriteQuote(int id)
    {
        var updatedCacheQuote = await quotesApi
            .FavoriteQuote(id)
            .ToCacheUpdateTask(_quoteLocalStorage, true);

        return updatedCacheQuote!.ToDomainModel();
    }

    public async Task<Quote> UnFavoriteQuote(int id)
    {
        var updatedCacheQuote = await quotesApi
            .UnFavoriteQuote(id)
            .ToCacheUpdateTask(_quoteLocalStorage, true);

        return updatedCacheQuote!.ToDomainModel();
    }

    public async Task<Quote> UpVoteQuote(int id)
    {
        var updatedCacheQuote = await quotesApi
            .UpVoteQuote(id)
            .ToCacheUpdateTask(_quoteLocalStorage);

        return updatedCacheQuote!.ToDomainModel();
    }

    public async Task<Quote> DownVoteQuote(int id)
    {
        var updatedCacheQuote = await quotesApi
            .DownVoteQuote(id)
            .ToCacheUpdateTask(_quoteLocalStorage);

        return updatedCacheQuote!.ToDomainModel();
    }

    public async Task<Quote> UnVoteQuote(int id)
    {
        var updatedCacheQuote = await quotesApi
            .UnVoteQuote(id)
            .ToCacheUpdateTask(_quoteLocalStorage);

        return updatedCacheQuote!.ToDomainModel();
    }

    public async Task ClearCache()
    {
        await _quoteLocalStorage.Clear();
    }
}

public static class QuoteRmExtension
{
    public static async Task<QuoteRm?> ToCacheUpdateTask(this Task<QuoteRm> quoteRmTask,
        QuoteLocalStorage localStorage,
        bool shouldInvalidateFavoritesCache = false)
    {
        try
        {
            var updatedApiQuote = await quoteRmTask;
            var updatedCacheQuote = updatedApiQuote.ToCacheModel();

            await Task.WhenAll([
                Task.Run(() => localStorage.UpdateQuote(updatedCacheQuote, !shouldInvalidateFavoritesCache)),
                shouldInvalidateFavoritesCache
                    ? Task.Run(() => localStorage.ClearQuoteListPageList(true))
                    : Task.CompletedTask
            ]);
            return updatedApiQuote;
        }
        catch (Exception ex)
        {
            if (ex is UserAuthRequiredQuoteException)
            {
                throw new UserAuthenticationRequiredException();
            }

            throw;
        }
    }
}

public enum QuoteListPageFetchPolicy
{
    CacheAndNetwork,
    NetworkOnly,
    NetworkPreferably,
    CachePreferably,
}