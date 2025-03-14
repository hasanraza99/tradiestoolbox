using System.Globalization;

namespace TradiesToolbox.Converters
{
    public class AmountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
            {
                if (amount > 0)
                    return Colors.Green; // Profit
                else if (amount < 0)
                    return Colors.Red; // Loss
                else
                    return Colors.Gray; // Break even
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}