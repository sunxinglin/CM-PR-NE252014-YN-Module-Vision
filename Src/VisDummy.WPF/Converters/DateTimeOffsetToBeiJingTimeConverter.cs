using System.Globalization;
using System.Windows.Data;

namespace VisDummy.WPF.Converters;

public class DateTimeOffsetToBeiJingTimeConverter : IValueConverter
{

    public static string ToBeiJingTime(DateTimeOffset? dto)
    {
        if (!dto.HasValue)
        {
            return string.Empty;
        }
        var bjt = dto.Value.ToOffset(TimeSpan.FromHours(8));
        return bjt.ToString();
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => ToBeiJingTime((DateTimeOffset?)value);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (DateTimeOffset.TryParse((string?)value, out var result))
        {
            return result;
        }
        return default;
    }
}