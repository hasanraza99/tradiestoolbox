using System.Globalization;
using TradiesToolbox.Models;

namespace TradiesToolbox.Converters
{
    public class JobStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JobStatus status)
            {
                return status switch
                {
                    JobStatus.Quoted => Colors.Gray,
                    JobStatus.Scheduled => Colors.DodgerBlue,
                    JobStatus.InProgress => Colors.Orange,
                    JobStatus.Completed => Colors.Green,
                    JobStatus.Invoiced => Colors.Purple,
                    JobStatus.Paid => Colors.DarkGreen,
                    JobStatus.Canceled => Colors.Red,
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