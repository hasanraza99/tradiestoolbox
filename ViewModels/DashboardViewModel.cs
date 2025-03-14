using System.Collections.ObjectModel;
using TradiesToolbox.Models;

namespace TradiesToolbox.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<JobItem> TodayJobs { get; } = new();
        public ObservableCollection<ActivityItem> RecentActivities { get; } = new();

        private string _welcomeMessage = "Welcome back!";
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        // LoadData method to initialize or refresh data
        public void LoadData()
        {
            // Simulated loading of jobs and activities (replace with actual data retrieval logic)
            TodayJobs.Clear();
            TodayJobs.Add(new JobItem { Time = "9:00 AM", Title = "Kitchen Installation", Duration = "2 hrs", ClientInfo = "John Smith - 123 Main St" });
            TodayJobs.Add(new JobItem { Time = "11:30 AM", Title = "Electrical Wiring", Duration = "1.5 hrs", ClientInfo = "Jane Doe - 456 Elm St" });

            RecentActivities.Clear();
            RecentActivities.Add(new ActivityItem { Description = "Quote #1052 was accepted", Timestamp = "Today", Color = "Green" });
            RecentActivities.Add(new ActivityItem { Description = "Invoice #2081 was paid", Timestamp = "Yesterday", Color = "Blue" });

            // Optionally, update any other UI-bound properties here
            WelcomeMessage = "Welcome back! Here’s your overview.";
        }
    }
}
