using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradiesToolbox.Converters
{
    /// <summary>
    /// Converts a date to a relative time string (Today, Yesterday, or the date)
    /// </summary>
    public class RelativeDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date.Date == DateTime.Today)
                {
                    return "Today";
                }
                else if (date.Date == DateTime.Today.AddDays(-1))
                {
                    return "Yesterday";
                }
                else if (date.Date > DateTime.Today.AddDays(-7))
                {
                    return date.ToString("dddd"); // Day name
                }
                else
                {
                    return date.ToString("MMM d"); // Month and day
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
