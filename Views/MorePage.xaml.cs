using TradiesToolbox.Services;

namespace TradiesToolbox.Views
{
    public partial class MorePage : ContentPage
    {
        public MorePage()
        {
            InitializeComponent();

            // Bind the switch to the current theme state
            DarkModeSwitch.IsToggled = ThemeService.IsDarkModeEnabled;

            // Find the Logout button and attach event handler
            var logoutButton = this.FindByName<Button>("LogoutButton");
            if (logoutButton != null)
            {
                logoutButton.Clicked += OnLogoutClicked;
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");

            if (confirm)
            {
                var supabaseService = new SupabaseService();
                await supabaseService.SignOutAsync();

                Application.Current.MainPage = new LoginPage();
            }
        }

        private void OnDarkModeSwitchToggled(object sender, ToggledEventArgs e)
        {
            // Apply the theme based on the switch state
            ThemeService.IsDarkModeEnabled = e.Value;
        }
    }
}