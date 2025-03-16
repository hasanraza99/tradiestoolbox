using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradiesToolbox.Converters
{
    /// <summary>
    /// Inverts a boolean value (true becomes false, false becomes true)
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return true; // Default to true if not a boolean
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false; // Default to false if not a boolean
        }
    }
}
