using System.Globalization;
using DomainModels;

namespace QuoteList.Converters;

public class IsTagSelectedBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(parameter);

        if (value is null)
            return false;

        return (Tag)value == (Tag)parameter;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}