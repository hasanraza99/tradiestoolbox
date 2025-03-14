using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    public partial class ClientsPage : ContentPage
    {
        // Private reference to ViewModel instance
        private readonly ClientsViewModel _viewModel;

        public ClientsPage()
        {
            // Initialize UI components from XAML
            InitializeComponent();

            // Assign the ViewModel to the BindingContext
            _viewModel = new ClientsViewModel();
            BindingContext = _viewModel;
        }

        // Called when the page appears on the screen
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh client list when page is displayed
            _viewModel.RefreshCommand.Execute(null);
        }
    }
}
