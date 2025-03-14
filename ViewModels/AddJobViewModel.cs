using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Data;
using TradiesToolbox.Models;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    public class AddJobViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();

        public Job Job { get; set; } = new Job
        {
            ScheduledDate = DateTime.Today, // Default to today
            Duration = TimeSpan.Zero, // Default duration
            Status = (int)JobStatus.Quoted // Default status
        };

        // Separate properties to handle hours and minutes input
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

        public AddJobViewModel()
        {
            Title = "Add Job";

            SaveJobCommand = new Command(async () => await SaveJob());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task SaveJob()
        {
            if (string.IsNullOrWhiteSpace(Job.Title))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Job title is required.", "OK");
                return;
            }

            if (!int.TryParse(DurationHours, out int hours)) hours = 0;
            if (!int.TryParse(DurationMinutes, out int minutes)) minutes = 0;
            Job.Duration = new TimeSpan(hours, minutes, 0);

            if (Enum.TryParse(SelectedStatus, out JobStatus status))
            {
                Job.Status = (int)status;
            }

            _jobDatabase.AddJob(Job);
            await Shell.Current.GoToAsync(".."); // Navigate back after saving
        }
    }
}
