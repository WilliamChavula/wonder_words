using System.Globalization;
using DomainModels;
using QuoteList.Extensions;

namespace QuoteList.Converters;

public class TagToLocalizedString : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        return ((Tag)value).ToLocalizedString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}