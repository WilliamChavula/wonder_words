using LocalStorage.Models;
using Realms;

namespace LocalStorage;

public class LocalStorage
{
    private const string QuoteListPagesRealmKey = "quote-list-pages";
    private const string FavoriteQuoteListPagesRealmKey = "favorite-quote-list-pages";
    private const string DarkModePreferenceRealmKey = "dark-mode-preference";

    public Realm GetFavoriteQuoteListPageRealm => GetInstance(FavoriteQuoteListPagesRealmKey);
    public Realm GetDarkModePreferenceRealm => GetInstance(DarkModePreferenceRealmKey);
    public Realm GetQuoteListPageRealm => GetInstance(QuoteListPagesRealmKey);

    private static Realm GetInstance(string realmName)
    {
        var config = new RealmConfiguration("quote-list-pages");

        return Realm.GetInstance(config);
    }
}