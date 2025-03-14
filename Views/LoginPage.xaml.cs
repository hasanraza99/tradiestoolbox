using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    public partial class LoginPage : ContentPage
    {
        // Private field storing ViewModel reference
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            // Initialize UI components from XAML
            InitializeComponent();

            // Assign the ViewModel to the BindingContext
            _viewModel = new LoginViewModel();
            BindingContext = _viewModel;
        }

        // Called when the page appears on the screen
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Initialize ViewModel logic
            _viewModel.OnLoginClicked();
        }
    }
}