using TradiesToolbox.Services;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Ensure database connection is established
                var dbConn = DBConnection.GetConnection();

                // Wait for 2 seconds to simulate splash screen delay
                await Task.Delay(2000);

                // Force logout during development if needed (remove this in production)
                // AuthService.Logout();

                // Check if the user is logged in
                bool isLoggedIn = AuthService.IsLoggedIn();
                Console.WriteLine($"User login status: {isLoggedIn}");

                // Navigate based on login status
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SplashPage: {ex.Message}");

                // Fallback to login page on error
                Application.Current.MainPage = new LoginPage();
            }
        }
    }
}
