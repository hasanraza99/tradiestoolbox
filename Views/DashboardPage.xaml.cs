using TradiesToolbox.ViewModels;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.Views
{
    public partial class DashboardPage : ContentPage
    {
        // Reference to the ViewModel instance for data binding
        private readonly DashboardViewModel _viewModel;

        public DashboardPage()
        {
            // Initialize UI components defined in XAML
            InitializeComponent();

            // Assign a new instance of the ViewModel
            _viewModel = new DashboardViewModel();

            // Set the BindingContext to the ViewModel to enable data binding
            BindingContext = _viewModel;
        }

        // Triggered when the page is about to appear on the screen
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Instruct the ViewModel to load data when the page is displayed
            _viewModel.LoadData();
        }
    }
}