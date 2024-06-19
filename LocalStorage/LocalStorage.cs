using LocalStorage.Models;
using Realms;

namespace LocalStorage;

public class LocalStorage
{
    private const string QuoteListPagesRealmKey = "quote-list-pages.realm";
    private const string FavoriteQuoteListPagesBoxKey = "favorite-quote-list-pages";
    private const string DarkModePreferenceBoxKey = "dark-mode-preference";

    public Task<Realm> GetFavoriteQuoteListPageBox => GetInstance(FavoriteQuoteListPagesBoxKey);
    public Task<Realm> GetDarkModePreferenceBox => GetInstance(DarkModePreferenceBoxKey);
    public Task<Realm> GetQuoteListPageBox => GetInstance(QuoteListPagesRealmKey);

    private static Task<Realm> GetInstance(string realmName)
    {
        var config = new RealmConfiguration(realmName);

        return Realm.GetInstanceAsync(config);
    }
}