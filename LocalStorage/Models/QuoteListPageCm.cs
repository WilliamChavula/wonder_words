using Realms;

namespace LocalStorage.Models;

public class QuoteListPageCm : RealmObject
{
    public int? PageNumber { get; set; }
    public bool IsLastPage { get; set; }
    public IList<QuoteCm> QuotesList { get; } = null!;
}
