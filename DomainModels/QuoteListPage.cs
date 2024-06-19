namespace DomainModels;

public record QuoteListPage(bool IsLastPage, IList<Quote> QuoteList);