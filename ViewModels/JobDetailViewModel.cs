// JobDetailViewModel.cs - Updated with additional functionality
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using TradiesToolbox.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace TradiesToolbox.ViewModels
{
    // This ViewModel receives the job ID through QueryProperties
    [QueryProperty(nameof(JobId), "id")]
    public class JobDetailViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();

        private Job _job;
        public Job Job
        {
            get => _job;
            set => SetProperty(ref _job, value);
        }

        private int _jobId;
        public int JobId
        {
            get => _jobId;
            set
            {
                _jobId = value;
                LoadJobAsync(_jobId).ConfigureAwait(false);
            }
        }

        // Property to control UI state of update button
        private bool _canUpdateStatus = false;
        public bool CanUpdateStatus
        {
            get => _canUpdateStatus;
            set => SetProperty(ref _canUpdateStatus, value);
        }

        // Status management properties
        public ObservableCollection<string> AvailableStatuses { get; } = new();

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SetProperty(ref _selectedStatus, value))
                {
                    // Enable update button if status changed
                    CanUpdateStatus = Job != null &&
                        Enum.TryParse(value, out JobStatus status) &&
                        (int)status != Job.Status;
                }
            }
        }

        // Commands for UI interactions
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand ViewClientCommand { get; }

        public JobDetailViewModel()
        {
            Title = "Job Details";
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);
            UpdateStatusCommand = new Command(OnUpdateStatus);
            ViewClientCommand = new Command(OnViewClient);

            // Initialize status list
            foreach (var status in Enum.GetNames(typeof(JobStatus)))
            {
                AvailableStatuses.Add(status);
            }
        }

        // Load job data from database using the ID
        public async Task LoadJobAsync(int jobId)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Job = _jobDatabase.GetJob(jobId);

                // Set the selected status to match current job status
                _selectedStatus = Enum.GetName(typeof(JobStatus), Job.Status);
                OnPropertyChanged(nameof(SelectedStatus));

                // Update page title
                Title = Job?.Title ?? "Job Details";

                // If job has a client ID, load client name
                if (Job.ClientID > 0)
                {
                    var client = _clientDatabase.GetClient(Job.ClientID);
                    Job.ClientName = client?.Name ?? "Unknown";
                }
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

        private async void OnEdit()
        {
            // Navigate to the Edit Job page with the job ID
            await Shell.Current.GoToAsync($"{nameof(EditJobPage)}?jobId={Job.JobID}");
        }


        // Delete the current job after confirmation
        private async void OnDelete()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Job",
                "Are you sure you want to delete this job?",
                "Yes", "No");

            if (confirm)
            {
                _jobDatabase.DeleteJob(Job.JobID);
                await Shell.Current.GoToAsync("..");
            }
        }

        // Update job status when selected status changes
        private async void OnUpdateStatus()
        {
            if (Enum.TryParse<JobStatus>(_selectedStatus, out JobStatus newStatus))
            {
                try
                {
                    // Update job status in memory and database
                    Job.Status = (int)newStatus;
                    _jobDatabase.UpdateJob(Job);
                    await Application.Current.MainPage.DisplayAlert(
                        "Status Updated",
                        $"Job status updated to {_selectedStatus}",
                        "OK");

                    // Reset button state
                    CanUpdateStatus = false;
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Failed to update status: {ex.Message}",
                        "OK");
                }
            }
        }

        // Navigate to client details
        private async void OnViewClient()
        {
            if (Job?.ClientID > 0)
            {
                await Shell.Current.GoToAsync($"{nameof(ClientDetailPage)}?id={Job.ClientID}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Client Not Found",
                    "This job is not associated with a client.",
                    "OK");
            }
        }
    }
}