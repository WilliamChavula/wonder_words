using System.Globalization;
using System.Windows.Input;

namespace QuoteDetails.Converters;

public class BooleanToCommandConverter : BindableObject, IValueConverter
{
    public static readonly BindableProperty FirstCommandOptionProperty = BindableProperty.Create(
        nameof(FirstCommandOption),
        typeof(ICommand),
        typeof(BooleanToCommandConverter)
    );
    public static readonly BindableProperty SecondCommandOptionProperty = BindableProperty.Create(
        nameof(SecondCommandOption),
        typeof(ICommand),
        typeof(BooleanToCommandConverter)
    );
    public ICommand? FirstCommandOption
    {
        get => (ICommand)GetValue(FirstCommandOptionProperty);
        set => SetValue(FirstCommandOptionProperty, value);
    }
    public ICommand? SecondCommandOption
    {
        get => (ICommand)GetValue(SecondCommandOptionProperty);
        set => SetValue(SecondCommandOptionProperty, value);
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(FirstCommandOption);
        ArgumentNullException.ThrowIfNull(SecondCommandOption);

        return (bool)value ? FirstCommandOption : SecondCommandOption;
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
