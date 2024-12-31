using System.Globalization;
using SignUp.ViewModels;

namespace SignUp.Converters;

public class SubmissionInProgressConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        return (SubmissionStatus)value is SubmissionStatus.InProgress;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}