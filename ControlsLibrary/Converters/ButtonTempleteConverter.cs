using System.Globalization;

namespace ControlsLibrary.Converters;

public class ButtonTempleteConverter : IValueConverter
{
    public View? DefaultButtonTemplate { get; set; }
    public View? AlternateButtonTemplate { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(DefaultButtonTemplate);
        ArgumentNullException.ThrowIfNull(AlternateButtonTemplate);

        return value is not null ? DefaultButtonTemplate : AlternateButtonTemplate;
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
