namespace CovrMe.Bahaviors;
public class LabelColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        bool isSelected = (bool)value;
        return isSelected ? Color.FromHex("#037672") : Color.FromHex("#424656");
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
