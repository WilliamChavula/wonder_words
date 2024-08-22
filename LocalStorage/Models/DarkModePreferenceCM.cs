using Realms;

namespace LocalStorage.Models;

public class DarkModePreferenceCm : RealmObject
{
    private int ThemeMode { get; set; }

    public ThemeModePreference ModePreference
    {
        get => (ThemeModePreference)ThemeMode;
        set => ThemeMode = (int)value;
    }
}

public enum ThemeModePreference
{
    Light,
    Dark,
    Unspecified
}
