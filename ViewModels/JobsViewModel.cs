using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;
using TradiesToolbox.Views;

namespace TradiesToolbox.ViewModels
{
    public class JobsViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();

        private ObservableCollection<Job> _jobs = new();
        public ObservableCollection<Job> Jobs
        {
            get => _jobs;
            set => SetProperty(ref _jobs, value);
        }

        // Add these missing properties
        public ObservableCollection<string> StatusFilters { get; } = new();
        public string SelectedStatusFilter { get; set; }
        public ObservableCollection<string> DateFilters { get; } = new();
        public string SelectedDateFilter { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand AddJobCommand { get; }
        public ICommand JobSelectedCommand { get; }

        public JobsViewModel()
        {
            Title = "Jobs";
            RefreshCommand = new Command(async () => await LoadJobsAsync());
            AddJobCommand = new Command(OnAddJob);
            JobSelectedCommand = new Command<Job>(OnJobSelected);

            // Initialize filters
            StatusFilters.Add("All Statuses");
            foreach (var status in Enum.GetNames(typeof(JobStatus)))
            {
                StatusFilters.Add(status);
            }
            SelectedStatusFilter = "All Statuses";

            DateFilters.Add("All Dates");
            DateFilters.Add("Today");
            DateFilters.Add("This Week");
            DateFilters.Add("This Month");
            SelectedDateFilter = "All Dates";

            LoadJobsAsync().ConfigureAwait(false);
        }

        private async Task LoadJobsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var jobList = _jobDatabase.GetJobs();
                var clientDb = new ClientDatabase();

                foreach (var job in jobList)
                {
                    var client = clientDb.GetClient(job.ClientID);
                    job.ClientName = client?.Name ?? "Unknown";
                }

                Jobs = new ObservableCollection<Job>(jobList);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnAddJob()
        {
            await Shell.Current.GoToAsync(nameof(AddJobPage));
        }

        private async void OnJobSelected(Job job)
        {
            if (job == null) return;
            await Shell.Current.GoToAsync($"{nameof(JobDetailPage)}?jobId={job.JobID}");
        }
    }
}
