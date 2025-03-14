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
        private readonly ClientDatabase _clientDatabase = new(); // Added client database reference

        private ObservableCollection<Job> _jobs = new();
        public ObservableCollection<Job> Jobs
        {
            get => _jobs;
            set => SetProperty(ref _jobs, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddJobCommand { get; }
        public ICommand JobSelectedCommand { get; }

        public JobsViewModel()
        {
            Title = "Jobs";
            RefreshCommand = new Command(async () => await LoadJobsAsync());
            AddJobCommand = new Command(OnAddJob);
            JobSelectedCommand = new Command<Job>(OnJobSelected);
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
                    job.ClientName = client?.Name ?? "Unknown"; // Assigning client name
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

            // Navigate to JobDetailPage and pass the JobID as a query parameter
            await Shell.Current.GoToAsync($"{nameof(JobDetailPage)}?jobId={job.JobID}");
        }
    }
}
