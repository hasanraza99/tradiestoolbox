using System.Collections.ObjectModel;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Views;
using TradiesToolbox.Data;
using TradiesToolbox.Services;
using TradiesToolbox.Enums;

namespace TradiesToolbox.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();
        private readonly QuoteDatabase _quoteDatabase = new();
        private readonly SupabaseService _supabaseService;

        // Collections for dashboard data
        public ObservableCollection<JobItem> TodayJobs { get; } = new();
        public ObservableCollection<ActivityItem> RecentActivities { get; } = new();

        // Dashboard statistical properties
        private int _totalJobs;
        public int TotalJobs
        {
            get => _totalJobs;
            set => SetProperty(ref _totalJobs, value);
        }

        private decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        private int _pendingQuotes;
        public int PendingQuotes
        {
            get => _pendingQuotes;
            set => SetProperty(ref _pendingQuotes, value);
        }

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
            _supabaseService = new SupabaseService();

            // Initialize commands
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

                // Personalize welcome message first
                PersonalizeWelcomeMessage();

                // Load today's jobs
                LoadTodayJobs();

                // Load recent activities
                LoadRecentActivities();

                // Calculate dashboard statistics
                CalculateDashboardStats();

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

        private void PersonalizeWelcomeMessage()
        {
            try
            {
                var userEmail = _supabaseService.GetCurrentUserEmail();
                if (!string.IsNullOrEmpty(userEmail))
                {
                    string username = userEmail.Contains('@') ?
                        userEmail.Split('@')[0] :
                        userEmail;

                    WelcomeMessage = GetTimeBasedGreeting() + $" {username}!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error personalizing welcome message: {ex.Message}");
            }
        }

        private string GetTimeBasedGreeting()
        {
            var currentHour = DateTime.Now.Hour;
            return currentHour switch
            {
                >= 5 and < 12 => "Good morning",
                >= 12 and < 17 => "Good afternoon",
                >= 17 and < 21 => "Good evening",
                _ => "Good night"
            };
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

        private void CalculateDashboardStats()
        {
            try
            {
                var jobs = _jobDatabase.GetJobs();
                var quotes = _quoteDatabase.GetQuotes();

                TotalJobs = jobs.Count;
                TotalRevenue = jobs
                    .Where(j => j.JobStatusEnum == JobStatus.Paid)
                    .Sum(j => j.ActualCost);
                PendingQuotes = quotes
                    .Count(q => q.StatusEnum == QuoteStatus.Draft || q.StatusEnum == QuoteStatus.Sent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating dashboard stats: {ex.Message}");
            }
        }
    }
}