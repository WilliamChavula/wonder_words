using DomainModels;
using LocalStorage.Models;

namespace QuoteRepository.Mappers;

public static class CacheToDomain
{
    public static Quote ToDomainModel(this QuoteCm quoteCm)
    {
        return new Quote
        {
            Id = quoteCm.Id,
            Body = quoteCm.Body,
            Author = quoteCm.Author,
            FavoriteCount = quoteCm.FavoriteCount,
            UpVotesCount = quoteCm.UpVoteCount,
            DownVotesCount = quoteCm.DownVoteCount,
            IsUpVoted = quoteCm.IsUpVoted,
            IsDownVoted = quoteCm.IsDownVoted,
            IsFavorite = quoteCm.IsFavorite
        };
    }

    public static QuoteListPage ToDomainModel(this QuoteListPageCm quoteListPageCm)
    {
        var quotes = quoteListPageCm.QuotesList.Select(quotes => quotes.ToDomainModel()).ToList();
        return new QuoteListPage(IsLastPage: quoteListPageCm.IsLastPage, QuoteList: quotes,
            PageNumber: quoteListPageCm.PageNumber);
    }
}