using LocalStorage.Models;
using Realms;

namespace LocalStorage;

public class LocalStorage
{
    private const string QuoteListPagesRealmKey = "quote-list-pages.realm";
    private const string FavoriteQuoteListPagesRealmKey = "favorite-quote-list-pages.realm";
    private const string DarkModePreferenceRealmKey = "dark-mode-preference.realm";
    private readonly RealmConfiguration darkModeConfuguration =
        new(DarkModePreferenceRealmKey) { Schema = new[] { typeof(DarkModePreferenceCm) } };

    private readonly RealmConfiguration quoteListPagesConfuguration =
        new(QuoteListPagesRealmKey) { Schema = new[] { typeof(QuoteListPageCm), typeof(QuoteCm) } };

    private readonly RealmConfiguration favoriteQuoteListPagesConfuguration =
        new(FavoriteQuoteListPagesRealmKey)
        {
            Schema = new[] { typeof(QuoteListPageCm), typeof(QuoteCm) }
        };
    public Realm GetFavoriteQuoteListPageRealm =>
        Realm.GetInstance(favoriteQuoteListPagesConfuguration);
    public Realm GetDarkModePreferenceRealm => Realm.GetInstance(darkModeConfuguration);
    public Realm GetQuoteListPageRealm => Realm.GetInstance(quoteListPagesConfuguration);
}
