using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Data;
using TradiesToolbox.Models;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    [QueryProperty(nameof(JobId), "jobId")]
    public class EditJobViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();

        public Job Job { get; set; } = new Job();

        // Collection of clients for selection
        public ObservableCollection<Client> Clients { get; } = new();

        // Currently selected client
        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (SetProperty(ref _selectedClient, value) && value != null)
                {
                    Job.ClientID = value.Id;
                    Job.ClientName = value.Name;
                }
            }
        }

        // Property to accept job id from navigation
        private int _jobId;
        public int JobId
        {
            get => _jobId;
            set
            {
                _jobId = value;
                if (value > 0)
                {
                    // Load the job with this ID
                    LoadJobAsync(value).ConfigureAwait(false);
                }
            }
        }

        // Duration handling for easier input
        public string DurationHours { get; set; }
        public string DurationMinutes { get; set; }

        // List of available job statuses for the Picker
        public ObservableCollection<string> AvailableStatuses { get; } =
            new ObservableCollection<string>(Enum.GetNames(typeof(JobStatus)));

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public ICommand SaveJobCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor initializes commands and data
        public EditJobViewModel()
        {
            Title = "Edit Job";
            SaveJobCommand = new Command(async () => await SaveJob());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            // Load available clients
            LoadClients();
        }

        // Load client data for dropdown
        private void LoadClients()
        {
            var clients = _clientDatabase.GetClients();
            foreach (var client in clients)
            {
                Clients.Add(client);
            }
        }

        // Load job data
        private async Task LoadJobAsync(int jobId)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Get job from database
                Job = _jobDatabase.GetJob(jobId);

                // Set duration values
                DurationHours = Job.Duration.Hours.ToString();
                DurationMinutes = Job.Duration.Minutes.ToString();

                // Set selected status
                _selectedStatus = Enum.GetName(typeof(JobStatus), Job.Status);
                OnPropertyChanged(nameof(SelectedStatus));

                // Find and select the client
                if (Job.ClientID > 0)
                {
                    foreach (var client in Clients)
                    {
                        if (client.Id == Job.ClientID)
                        {
                            SelectedClient = client;
                            break;
                        }
                    }
                }

                // Update title with job name
                Title = $"Edit: {Job.Title}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading job: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load job: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Save job to database
        private async Task SaveJob()
        {
            if (string.IsNullOrWhiteSpace(Job.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Job title is required.", "OK");
                return;
            }

            if (SelectedClient == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a client.", "OK");
                return;
            }

            // Parse duration values
            if (!int.TryParse(DurationHours, out int hours)) hours = 0;
            if (!int.TryParse(DurationMinutes, out int minutes)) minutes = 0;
            Job.Duration = new TimeSpan(hours, minutes, 0);

            // Set status from selection
            if (Enum.TryParse(SelectedStatus, out JobStatus status))
            {
                Job.Status = (int)status;
            }

            try
            {
                _jobDatabase.UpdateJob(Job);
                await Shell.Current.GoToAsync(".."); // Navigate back after saving
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save job: {ex.Message}", "OK");
            }
        }
    }
}