using System.Globalization;

namespace CovrMe.Handlers
{
    public class MoreThanOneUserToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count && count > 1)
            {
                return true; // Auto-check the checkbox if more than one user
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
