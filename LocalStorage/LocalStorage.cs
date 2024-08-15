using Realms;

namespace LocalStorage;

public class LocalStorage
{
    private const string QuoteListPagesRealmKey = "quote-list-pages.realm";
    private const string FavoriteQuoteListPagesRealmKey = "favorite-quote-list-pages.realm";
    private const string DarkModePreferenceRealmKey = "dark-mode-preference.realm";

    public Realm GetFavoriteQuoteListPageRealm => GetInstance(FavoriteQuoteListPagesRealmKey);
    public Realm GetDarkModePreferenceRealm => GetInstance(DarkModePreferenceRealmKey);
    public Realm GetQuoteListPageRealm => GetInstance(QuoteListPagesRealmKey);

    private static Realm GetInstance(string realmName)
    {
        var config = new RealmConfiguration(realmName);

        return Realm.GetInstance(config);
    }
}