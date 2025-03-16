using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradiesToolbox.Converters
{
    /// <summary>
    /// Converts a string to a boolean - returns true if the string is not null or empty
    /// </summary>
    public class StringNotEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return !string.IsNullOrWhiteSpace(stringValue);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
