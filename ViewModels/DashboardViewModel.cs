using System.Collections.ObjectModel;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Views;
using TradiesToolbox.Data;

namespace TradiesToolbox.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();
        private readonly QuoteDatabase _quoteDatabase = new();

        public ObservableCollection<JobItem> TodayJobs { get; } = new();
        public ObservableCollection<ActivityItem> RecentActivities { get; } = new();

        // Property for XAML binding
        public int TodayJobsCount => TodayJobs?.Count ?? 0;

        // Commands for XAML binding
        public ICommand NewJobCommand { get; }
        public ICommand NewQuoteCommand { get; }
        public ICommand ViewAllJobsCommand { get; }

        private string _welcomeMessage = "Welcome back!";
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        public DashboardViewModel()
        {
            Title = "Dashboard";

            // Initialize commands
            NewJobCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(AddJobPage)));
            NewQuoteCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(AddQuotePage)));
            ViewAllJobsCommand = new Command(async () => await Shell.Current.GoToAsync("//JobsPage"));
        }

        public void LoadData()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Load today's jobs
                TodayJobs.Clear();
                var todayJobs = _jobDatabase.GetJobs()
                    .Where(j => j.ScheduledDate.Date == DateTime.Today);

                foreach (var job in todayJobs)
                {
                    var client = _clientDatabase.GetClient(job.ClientID);
                    string clientName = client?.Name ?? "Unknown";

                    TodayJobs.Add(new JobItem
                    {
                        Time = job.ScheduledDate.ToString("h:mm tt"),
                        Title = job.Title,
                        Duration = $"{job.Duration.Hours}h {job.Duration.Minutes}m",
                        ClientInfo = $"{clientName} - {job.Location}"
                    });
                }

                // Load recent activities
                RecentActivities.Clear();

                // Recent jobs
                var recentJobs = _jobDatabase.GetJobs()
                    .OrderByDescending(j => j.CreatedDate)
                    .Take(3);

                foreach (var job in recentJobs)
                {
                    string timeAgo = job.CreatedDate.Date == DateTime.Today ? "Today" :
                                     job.CreatedDate.Date == DateTime.Today.AddDays(-1) ? "Yesterday" :
                                     job.CreatedDate.ToString("MMM d");

                    RecentActivities.Add(new ActivityItem
                    {
                        Description = $"Job: {job.Title} ({job.JobStatusEnum})",
                        Timestamp = timeAgo,
                        Color = "Gray" // Converter will handle this on UI
                    });
                }

                // Recent quotes
                var recentQuotes = _quoteDatabase.GetQuotes().Take(3);
                foreach (var quote in recentQuotes)
                {
                    RecentActivities.Add(new ActivityItem
                    {
                        Description = $"Quote: {quote.JobTitle} ({quote.StatusDisplay})",
                        Timestamp = "Recently",
                        Color = "Blue" // Converter will handle this on UI
                    });
                }

                // Personalize welcome message
                string userEmail = Preferences.Get("UserEmail", string.Empty);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    WelcomeMessage = $"Welcome back, {userEmail.Split('@')[0]}!";
                }

                OnPropertyChanged(nameof(TodayJobsCount));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dashboard data: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}