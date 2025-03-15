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

        // Add this to the DashboardPage.xaml.cs
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Console.WriteLine("DashboardPage.OnAppearing - Loading data");
                _viewModel.LoadData();
                Console.WriteLine("DashboardPage.OnAppearing - Data loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DashboardPage.OnAppearing: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}