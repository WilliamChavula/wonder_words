using Realms;

namespace LocalStorage.Models;

public partial class QuoteListPageCm : RealmObject
{
    public bool IsLastPage { get; set; }
    public IList<QuotesCm> QuotesList { get; }

    public QuoteListPageCm()
    {
    }

    public QuoteListPageCm(bool isLastPage, List<QuotesCm> quotesList)
    {
        IsLastPage = isLastPage;
        QuotesList = quotesList;
    }
}