using TradiesToolbox.Services;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.Views
{
    public partial class SplashPage : ContentPage
    {
        private readonly SupabaseService _supabaseService;

        public SplashPage()
        {
            InitializeComponent();
            _supabaseService = new SupabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Ensure database connection is established
                var dbConn = DBConnection.GetConnection();

                // Wait for 3 seconds to allow splash animations to complete
                await Task.Delay(3000);

                // Check authentication status using Supabase
                if (_supabaseService.IsAuthenticated())
                {
                    // User is logged in, navigate to main app
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    // User is not authenticated, go to login page
                    Application.Current.MainPage = new LoginPage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SplashPage: {ex.Message}");
                await DisplayAlert("Error", "An error occurred during startup. The app will restart.", "OK");

                // Force logout and restart
                await _supabaseService.SignOutAsync();
                Application.Current.MainPage = new LoginPage();
            }
        }
    }
}