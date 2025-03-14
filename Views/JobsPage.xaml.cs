using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    // Code-behind file for JobsPage.xaml
    public partial class JobsPage : ContentPage
    {
        private JobsViewModel _viewModel; // ViewModel instance

        public JobsPage()
        {
            InitializeComponent();

            // Initialize ViewModel and set binding context
            _viewModel = new JobsViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh job listings when the page appears
            _viewModel.RefreshCommand.Execute(null);
        }
    }
}