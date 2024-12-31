using System.Globalization;

namespace QuoteDetails.Converters;

public class BooleanToColorConverter : IValueConverter
{
    public Color? FirstColor { get; set; }
    public Color? SecondColor { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(FirstColor);
        ArgumentNullException.ThrowIfNull(SecondColor);

        return (bool)value ? FirstColor : SecondColor;
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
