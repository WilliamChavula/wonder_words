namespace UserRepository;

public class UserLocalStorage(LocalStorage.LocalStorage localStorage)
{
    public void UpsertDarkModePreference(DarkModePreferenceCm preference)
    {
        var realm = localStorage.GetDarkModePreferenceRealm;

        realm.Write(() =>
        {
            realm.Add(preference);
            realm.Dispose();
        });
    }

    public DarkModePreferenceCm? GetDarkModePreference()
    {
        var realm = localStorage.GetDarkModePreferenceRealm;

        var result = realm.All<DarkModePreferenceCm>();
        if (!result.Any())
            return null;

        var preference = result.FirstOrDefault();

        realm.Dispose();

        return preference;
    }
}
