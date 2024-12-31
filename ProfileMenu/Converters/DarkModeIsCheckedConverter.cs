using System.Globalization;
using DomainModels;

namespace ProfileMenu.Converters;

public class DarkModeIsCheckedConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return false;
        if (parameter is null) return false;

        return Enum.Parse<DarkModePreference>((string)value) == (DarkModePreference)parameter;
    }

    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
