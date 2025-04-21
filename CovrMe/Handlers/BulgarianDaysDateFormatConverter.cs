using System.Globalization;

namespace CovrMe.Handlers
{
    public class BulgarianDaysDateFormatConverter : IValueConverter
    {
        private readonly CultureInfo bulgarianCulture = new CultureInfo("bg-BG");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("ddd, MMM dd", bulgarianCulture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Typically not needed for date formatting, so you can leave it unimplemented or throw NotImplementedException
            throw new NotImplementedException();
        }
    }
}
