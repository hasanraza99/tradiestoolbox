using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Data;
using TradiesToolbox.Models;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public class AddJobViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();

        public Job Job { get; set; } = new Job
        {
            ScheduledDate = DateTime.Today, // Default to today
            Duration = TimeSpan.Zero, // Default duration
            Status = (int)JobStatus.Quoted // Default status
        };

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

        // Property to accept client id from navigation
        private int _clientId;
        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                if (value > 0)
                {
                    // Find and select the client with this ID
                    LoadClientById(value);
                }
            }
        }

        // Duration handling for easier input
        public string DurationHours { get; set; } = "0";
        public string DurationMinutes { get; set; } = "0";

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
        public AddJobViewModel()
        {
            Title = "Add Job";
            SaveJobCommand = new Command(async () => await SaveJob());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));

            // Initialize selected status
            _selectedStatus = Enum.GetName(typeof(JobStatus), JobStatus.Quoted);

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

        private void LoadClientById(int clientId)
        {
            try
            {
                var client = _clientDatabase.GetClient(clientId);
                if (client != null)
                {
                    // Find this client in our collection
                    foreach (var c in Clients)
                    {
                        if (c.Id == clientId)
                        {
                            SelectedClient = c;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading client: {ex.Message}");
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

            // Set creation date
            Job.CreatedDate = DateTime.Now;

            try
            {
                _jobDatabase.AddJob(Job);
                await Shell.Current.GoToAsync(".."); // Navigate back after saving
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save job: {ex.Message}", "OK");
            }
        }
    }
}