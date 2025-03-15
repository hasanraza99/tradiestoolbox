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

                // Navigate based on login status
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SplashPage: {ex.Message}");
                await DisplayAlert("Error", "An error occurred during startup. The app will restart.", "OK");

                // Force logout and restart
                AuthService.Logout();
                Application.Current.MainPage = new LoginPage();
            }
        }
    }
}
