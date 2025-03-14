using TradiesToolbox.Services;
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

            // Wait for 2 seconds to simulate splash screen delay
            await Task.Delay(2000);

            // Check login status and navigate accordingly
            if (AuthService.IsLoggedIn())
            {
                Application.Current.MainPage = new AppShell(); // Go to dashboard
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(LoginPage)); // Go to login
            }
        }
    }
}
