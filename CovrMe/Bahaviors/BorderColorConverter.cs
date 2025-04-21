namespace CovrMe.Bahaviors;
public class BorderColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool isSelected = (bool)value;
        return isSelected ? Color.FromHex("#FFFFFF") : Color.FromHex("#D9D9D9");
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
