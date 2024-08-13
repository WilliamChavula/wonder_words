using LocalStorage.Models;
using QuotesApi.Models.Response;

namespace QuoteRepository.Mappers;

public static class ApiQuotesToCache
{
    public static QuoteListPageCm ToCacheModel(this QuoteListPageRm quoteListPageRm)
    {
        var pageListCm = new QuoteListPageCm
        {
            IsLastPage = quoteListPageRm.IsLastPage,
            PageNumber = quoteListPageRm.PageNumber
        };

        foreach (var quote in quoteListPageRm.QuoteList)
        {
            pageListCm.QuotesList.Add(quote.ToCacheModel());
        }
        
        return pageListCm;
    }

    public static QuoteCm ToCacheModel(this QuoteRm quoteRm)
    {
        return new QuoteCm
        {
            Id = quoteRm.Id,
            Body = quoteRm.Body ?? "",
            Author = quoteRm.Author,
            FavoriteCount = quoteRm.FavoritesCount,
            UpVoteCount = quoteRm.UpVotesCount,
            DownVoteCount = quoteRm.DownVotesCount,
            // IsFavorite = quoteRm.UserInfo?.IsFavorite,
            // IsUpVoted = quoteRm.UserInfo?.IsUpVoted,
            // IsDownVoted = quoteRm.UserInfo?.IsDownVoted
        };
    }
}