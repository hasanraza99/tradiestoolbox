using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    [QueryProperty(nameof(JobId), "jobId")]
    public partial class EditJobPage : ContentPage
    {
        private readonly EditJobViewModel _viewModel;

        // Property to receive job ID from navigation
        public int JobId
        {
            set
            {
                if (_viewModel != null)
                {
                    // Pass the job ID to the ViewModel
                    _viewModel.JobId = value;
                }
            }
        }

        public EditJobPage()
        {
            InitializeComponent();
            _viewModel = new EditJobViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}