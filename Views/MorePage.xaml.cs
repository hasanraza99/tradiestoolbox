using TradiesToolbox.Services;

namespace TradiesToolbox.Views
{
    public partial class MorePage : ContentPage
    {
        public MorePage()
        {
            InitializeComponent();

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
                AuthService.Logout();
                Application.Current.MainPage = new LoginPage();
            }
        }
    }
}