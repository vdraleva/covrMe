
namespace CovrMe.Handlers;

using System.Globalization;
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isError && isError)
        {
            return Color.FromHex("#ff0000");
        }
        return Color.FromHex("#000000"); // Default color
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
