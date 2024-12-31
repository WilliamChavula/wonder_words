using System.Globalization;

namespace QuoteDetails.Converters;

public class BooleanToIconConverter : IValueConverter
{
    public string? FirstIcon { get; set; }
    public string? SecondIcon { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(FirstIcon);
        ArgumentNullException.ThrowIfNull(SecondIcon);

        return (bool)value ? FirstIcon : SecondIcon;
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
