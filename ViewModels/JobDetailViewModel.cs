using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    [QueryProperty(nameof(JobId), "id")]
    public class JobDetailViewModel : BaseViewModel
    {
        private readonly JobDatabase _jobDatabase = new();

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

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public JobDetailViewModel()
        {
            Title = "Job Details";
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);
        }

        public async Task LoadJobAsync(int jobId)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Job = _jobDatabase.GetJob(jobId);
                Title = Job?.Title ?? "Job Details";
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
            await Application.Current.MainPage.DisplayAlert("Edit", "Navigate to Edit Job Page", "OK");
        }

        private async void OnDelete()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete Job", "Are you sure?", "Yes", "No");
            if (confirm)
            {
                _jobDatabase.DeleteJob(Job.JobID);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
