using System.Globalization;
using UpdateProfile.ViewModels;

namespace UpdateProfile.Converters;

public class SubmissionInProgressInvertedConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(value);
        return (SubmissionStatus)value is not SubmissionStatus.InProgress;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}