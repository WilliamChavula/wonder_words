using DomainModels;
using QuotesApi.Models.Response;

namespace QuoteRepository.Mappers;

public static class ApiQuotesToDomain
{
    public static Quote ToDomainModel(this QuoteRm quoteRm)
    {
        return new Quote
        {
            Id = quoteRm.Id,
            Body = quoteRm.Body ?? "",
            Author = quoteRm.Author,
            DownVotesCount = quoteRm.DownVotesCount,
            FavoriteCount = quoteRm.FavoritesCount,
            // IsDownVoted = quoteRm.UserInfo?.IsDownVoted,
            // IsFavorite = quoteRm.UserInfo?.IsFavorite,
            // IsUpVoted = quoteRm.UserInfo?.IsUpVoted,
            UpVotesCount = quoteRm.UpVotesCount
        };
    }

    public static QuoteListPage ToDomainModel(this QuoteListPageRm quoteListPageRm)
    {
        var quotes = quoteListPageRm.QuoteList.Select(quote => quote.ToDomainModel()).ToList();
        return new QuoteListPage(IsLastPage: quoteListPageRm.IsLastPage, QuoteList: quotes,
            PageNumber: quoteListPageRm.PageNumber);
    }
}