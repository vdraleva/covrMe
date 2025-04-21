using System.Globalization;

namespace CovrMe.Handlers
{
    public class InsuredUsersIosHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOpen && isOpen)
            {
                return 550; // Expanded height
            }
            return 60; // Collapsed height
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
