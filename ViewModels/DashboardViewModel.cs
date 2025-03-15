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

            NewJobCommand = new Command(async () => await NavigationHelper.SafeNavigateAsync(nameof(AddJobPage)));
            NewQuoteCommand = new Command(async () => await NavigationHelper.SafeNavigateAsync(nameof(AddQuotePage)));
            ViewAllJobsCommand = new Command(async () => await NavigationHelper.SafeNavigateAsync("//JobsPage"));
        }

        public void LoadData()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Console.WriteLine("Dashboard: Beginning to load data");
                // Load today's jobs
                LoadTodayJobs();

                // Load recent activities
                LoadRecentActivities();

                // Personalize welcome message
                PersonalizeWelcomeMessage();

                // Update UI properties
                OnPropertyChanged(nameof(TodayJobsCount));
                Console.WriteLine("Dashboard: Data loading completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dashboard data: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LoadTodayJobs()
        {
            try
            {
                TodayJobs.Clear();

                // Get all jobs and materialize the query with ToList()
                var allJobs = _jobDatabase.GetJobs();
                if (allJobs == null)
                {
                    Console.WriteLine("No jobs found in database");
                    return;
                }

                // Filter for today's jobs and materialize the query
                var todayScheduled = allJobs
                    .Where(j => j.ScheduledDate.Date == DateTime.Today)
                    .ToList();

                Console.WriteLine($"Found {todayScheduled.Count} jobs scheduled for today");

                foreach (var job in todayScheduled)
                {
                    try
                    {
                        if (job == null)
                        {
                            Console.WriteLine("Skipping null job entry");
                            continue;
                        }

                        // Safely get client name
                        string clientName = "Unknown";
                        string location = "Unknown location";

                        if (job.ClientID > 0)
                        {
                            try
                            {
                                var client = _clientDatabase.GetClient(job.ClientID);
                                clientName = client?.Name ?? "Unknown";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error fetching client for job {job.JobID}: {ex.Message}");
                            }
                        }

                        // Safely get job location
                        if (!string.IsNullOrEmpty(job.Location))
                        {
                            location = job.Location;
                        }

                        // Create job item with safe values
                        var jobItem = new JobItem
                        {
                            Time = job.ScheduledDate.ToString("h:mm tt"),
                            Title = !string.IsNullOrEmpty(job.Title) ? job.Title : "Untitled Job",
                            Duration = $"{job.Duration.Hours}h {job.Duration.Minutes}m",
                            ClientInfo = $"{clientName} - {location}"
                        };

                        // Add to collection
                        TodayJobs.Add(jobItem);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing job {job?.JobID}: {ex.Message}");
                        // Continue to next job instead of crashing
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadTodayJobs: {ex.Message}");
            }
        }

        private void LoadRecentActivities()
        {
            try
            {
                RecentActivities.Clear();

                // Recent jobs - limit to 3
                var recentJobs = _jobDatabase.GetJobs()
                    .OrderByDescending(j => j.CreatedDate)
                    .Take(3)
                    .ToList();  // Materialize the query

                foreach (var job in recentJobs)
                {
                    if (job == null) continue;

                    string timeAgo = "Recently";

                    try
                    {
                        timeAgo = job.CreatedDate.Date == DateTime.Today ? "Today" :
                                  job.CreatedDate.Date == DateTime.Today.AddDays(-1) ? "Yesterday" :
                                  job.CreatedDate.ToString("MMM d");
                    }
                    catch
                    {
                        // If date calculation fails, use default
                        timeAgo = "Recently";
                    }

                    RecentActivities.Add(new ActivityItem
                    {
                        Description = $"Job: {job.Title ?? "Untitled"} ({job.JobStatusEnum})",
                        Timestamp = timeAgo,
                        Color = "Gray" // Converter will handle this on UI
                    });
                }

                // Recent quotes - be defensive
                try
                {
                    var recentQuotes = _quoteDatabase.GetQuotes()
                        .Take(3)
                        .ToList();  // Materialize the query

                    foreach (var quote in recentQuotes)
                    {
                        if (quote == null) continue;

                        RecentActivities.Add(new ActivityItem
                        {
                            Description = $"Quote: {quote.JobTitle ?? "Untitled"} ({quote.StatusDisplay ?? "Unknown"})",
                            Timestamp = "Recently",
                            Color = "Blue" // Converter will handle this on UI
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading quotes: {ex.Message}");
                    // Continue execution instead of crashing
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadRecentActivities: {ex.Message}");
            }
        }

        private void PersonalizeWelcomeMessage()
        {
            try
            {
                string userEmail = Preferences.Get("UserEmail", string.Empty);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    string username = userEmail.Contains('@') ?
                        userEmail.Split('@')[0] :
                        userEmail;

                    WelcomeMessage = $"Welcome back, {username}!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error personalizing welcome message: {ex.Message}");
                // Keep default welcome message on error
            }
        }
    }
}