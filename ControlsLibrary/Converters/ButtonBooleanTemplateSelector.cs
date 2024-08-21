using System;
using System.Globalization;

namespace ControlsLibrary.Converters;

public class ButtonBooleanTemplateSelector : IValueConverter
{
    public View? IfTruthyView { get; set; }
    public View? IfFalseyView { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(IfTruthyView);
        ArgumentNullException.ThrowIfNull(IfFalseyView);

        return (bool)value ? IfTruthyView : IfFalseyView;
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
