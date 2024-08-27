using System.Globalization;

namespace ForgotMyPassword.Converters;

public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // ArgumentNullException.ThrowIfNull(value);

        if (value is null)
            return false;

        return !(bool)value;
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
