using Realms;

namespace LocalStorage.Models;

public partial class DarkModePreferenceCm : IRealmObject
{
    public int ThemeMode { get; set; }

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