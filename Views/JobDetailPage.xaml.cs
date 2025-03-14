using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    [QueryProperty(nameof(JobId), "jobId")]
    public partial class JobDetailPage : ContentPage
    {
        private readonly JobDetailViewModel _viewModel;

        private int _jobId;
        public int JobId
        {
            get => _jobId;
            set
            {
                _jobId = value;
                LoadJob(value); // Load job details as soon as jobId is set
            }
        }

        public JobDetailPage()
        {
            InitializeComponent();
            _viewModel = new JobDetailViewModel();
            BindingContext = _viewModel;
        }

        private async void LoadJob(int jobId)
        {
            await _viewModel.LoadJobAsync(jobId);
        }
    }
}
