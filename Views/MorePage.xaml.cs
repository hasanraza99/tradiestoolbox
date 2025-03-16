using TradiesToolbox.Services;

namespace TradiesToolbox.Views
{
    public partial class MorePage : ContentPage
    {
        private readonly SupabaseService _supabaseService;

        // Properties for binding
        public bool IsDarkModeEnabled { get; set; }
        public string UserEmail { get; set; }
        public string UserInitials { get; set; }

        public MorePage()
        {
            InitializeComponent();

            // Initialize services
            _supabaseService = new SupabaseService();

            // Get dark mode setting
            IsDarkModeEnabled = ThemeService.IsDarkModeEnabled;

            // Get user info
            GetUserInfo();

            // Set binding context to this instance
            BindingContext = this;

            // Find the Logout button and attach event handler
            var logoutButton = this.FindByName<Button>("LogoutButton");
            if (logoutButton != null)
            {
                logoutButton.Clicked += OnLogoutClicked;
            }
        }

        // Get user info for display
        private void GetUserInfo()
        {
            try
            {
                // Get user email from Supabase
                UserEmail = _supabaseService.GetCurrentUserEmail() ?? "user@example.com";

                // Create initials from email
                if (!string.IsNullOrEmpty(UserEmail) && UserEmail.Contains('@'))
                {
                    string name = UserEmail.Split('@')[0];
                    if (name.Length > 0)
                    {
                        // Get first character
                        UserInitials = name[0].ToString().ToUpper();

                        // If there's a period or underscore, get the first character after it
                        int separatorIndex = name.IndexOfAny(new[] { '.', '_' });
                        if (separatorIndex >= 0 && separatorIndex < name.Length - 1)
                        {
                            UserInitials += name[separatorIndex + 1].ToString().ToUpper();
                        }
                        // Otherwise, get the last character if different from first
                        else if (name.Length > 1)
                        {
                            UserInitials += name[name.Length - 1].ToString().ToUpper();
                        }
                    }
                    else
                    {
                        UserInitials = "TT"; // Default for Tradies Toolbox
                    }
                }
                else
                {
                    UserInitials = "TT"; // Default for Tradies Toolbox
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user info: {ex.Message}");
                UserEmail = "user@example.com";
                UserInitials = "TT";
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");

            if (confirm)
            {
                try
                {
                    await _supabaseService.SignOutAsync();
                    Application.Current.MainPage = new LoginPage();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to log out: {ex.Message}", "OK");
                }
            }
        }

        private void OnDarkModeSwitchToggled(object sender, ToggledEventArgs e)
        {
            // Apply the theme based on the switch state
            ThemeService.IsDarkModeEnabled = e.Value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh user info when page appears
            GetUserInfo();

            // Refresh UI
            OnPropertyChanged(nameof(UserEmail));
            OnPropertyChanged(nameof(UserInitials));
        }
    }
}