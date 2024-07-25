using System.Globalization;
using QuoteList.ViewModels;

namespace QuoteList.Converters;

public class FilterByFavoritesToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        return (QuoteListFilter)value is QuoteListFilter.QuoteListFilterByFavorites;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}