using LocalStorage.Models;
using Realms;

namespace LocalStorage;

public abstract class LocalStorage
{
    private const string QuoteListPagesRealmKey = "quote-list-pages.realm";
    private const string FavoriteQuoteListPagesRealmKey = "favorite-quote-list-pages";
    private const string DarkModePreferenceRealmKey = "dark-mode-preference";

    public Task<Realm> GetFavoriteQuoteListPageRealm => GetInstance(FavoriteQuoteListPagesRealmKey);
    public Task<Realm> GetDarkModePreferenceRealm => GetInstance(DarkModePreferenceRealmKey);
    public Task<Realm> GetQuoteListPageRealm => GetInstance(QuoteListPagesRealmKey);

    private static Task<Realm> GetInstance(string realmName)
    {
        var config = new RealmConfiguration(realmName);

        return Realm.GetInstanceAsync(config);
    }
}