using System.Globalization;

namespace kworking.Converters;

public class FromUserIdToBackgroudColorConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length == 0) return Color.FromArgb("#2BAD72");

        if (values[0] == null || values[1] == null) return Color.FromArgb("#2BAD72");

        if (values[0].ToString() == values[1].ToString()) return Color.FromArgb("#2BAD72");

        return Color.FromArgb("#ffffff");

    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
