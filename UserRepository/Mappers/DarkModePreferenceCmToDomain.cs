namespace UserRepository.Mappers;

public static class DarkModePreferenceCmToDomain
{
    public static DarkModePreference ToDomainModel(this DarkModePreferenceCm darkModePreference)
    {
        return darkModePreference.ModePreference switch
        {
            ThemeModePreference.Dark => DarkModePreference.Dark,
            ThemeModePreference.Light => DarkModePreference.Light,
            ThemeModePreference.Unspecified => DarkModePreference.Unspecified,
            _ => throw new ArgumentOutOfRangeException(nameof(darkModePreference), darkModePreference.ModePreference, null)
        };
    }
}