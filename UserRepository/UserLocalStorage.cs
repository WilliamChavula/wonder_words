namespace UserRepository;

public class UserLocalStorage(LocalStorage.LocalStorage localStorage)
{
    public async Task UpsertDarkModePreference(DarkModePreferenceCm preference)
    {
        var realm = await localStorage.GetDarkModePreferenceRealm;
        
        await realm.WriteAsync(() =>
        {
            realm.Add(preference);
        });
    }

    public async Task<DarkModePreferenceCm?> GetDarkModePreference()
    {
        var realm = await localStorage.GetDarkModePreferenceRealm;
        
        return realm.All<DarkModePreferenceCm>().FirstOrDefault();
    }
}