using System.Globalization;
using SignIn.ViewModels;

namespace SignIn.Converters;

public class SubmissionInProgressConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (SubmissionStatus)value! is SubmissionStatus.InProgress;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}