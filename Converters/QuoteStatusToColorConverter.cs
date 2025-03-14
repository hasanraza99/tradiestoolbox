using System.Globalization;
using TradiesToolbox.Enums;

namespace TradiesToolbox.Converters
{
    public class QuoteStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuoteStatus status)
            {
                return status switch
                {
                    QuoteStatus.Draft => Colors.Gray,
                    QuoteStatus.Sent => Colors.DodgerBlue,
                    QuoteStatus.Accepted => Colors.Green,
                    QuoteStatus.Rejected => Colors.Red,
                    QuoteStatus.Expired => Colors.Orange,
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
