namespace UserRepository.Mappers;

public static class DarkModePreferenceDomainToCm
{
    public static DarkModePreferenceCm ToCacheModel(this DarkModePreference darkModePreference)
    {
        return darkModePreference switch
        {
            DarkModePreference.Dark => new DarkModePreferenceCm{ ModePreference = ThemeModePreference.Dark},
            DarkModePreference.Light => new DarkModePreferenceCm{ ModePreference = ThemeModePreference.Light},
            DarkModePreference.Unspecified => new DarkModePreferenceCm{ ModePreference = ThemeModePreference.Unspecified},
            _ => throw new ArgumentOutOfRangeException(nameof(darkModePreference), darkModePreference, null)
        };
    }
}