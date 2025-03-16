using Microsoft.Maui.Controls;

namespace TradiesToolbox.Services
{
    public static class ThemeService
    {
        private const string ThemeKey = "AppTheme";

        public static bool IsDarkModeEnabled
        {
            get => Preferences.Default.Get(ThemeKey, false);
            set
            {
                Preferences.Default.Set(ThemeKey, value);
                ApplyTheme();
            }
        }

        public static void ApplyTheme()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var application = Application.Current;
                if (application == null) return;

                // Set the app's theme mode
                application.UserAppTheme = IsDarkModeEnabled ? AppTheme.Dark : AppTheme.Light;

                if (IsDarkModeEnabled)
                {
                    // Apply dark theme colors
                    application.Resources["PrimaryColor"] = Color.FromArgb("#BF4D11"); // Toolbox rust color
                    application.Resources["PrimaryDarkColor"] = Color.FromArgb("#8C3A0D"); // Darker rust
                    application.Resources["AccentColor"] = Color.FromArgb("#E5A553"); // Background amber
                    application.Resources["BackgroundColor"] = Color.FromArgb("#1A1A1A"); // Near black
                    application.Resources["TextColor"] = Color.FromArgb("#FFFFFF"); // White text
                    application.Resources["TextSecondaryColor"] = Color.FromArgb("#E5A553"); // Amber for secondary text
                    application.Resources["BorderColor"] = Color.FromArgb("#333333"); // Dark gray border
                }
                else
                {
                    // Apply light theme colors
                    application.Resources["PrimaryColor"] = Color.FromArgb("#BF4D11"); // Toolbox rust color
                    application.Resources["PrimaryDarkColor"] = Color.FromArgb("#8C3A0D"); // Darker rust
                    application.Resources["AccentColor"] = Color.FromArgb("#E5A553"); // Background amber
                    application.Resources["BackgroundColor"] = Color.FromArgb("#FCFAF5"); // Off-white background
                    application.Resources["TextColor"] = Color.FromArgb("#111111"); // Near black text
                    application.Resources["TextSecondaryColor"] = Color.FromArgb("#7D3109"); // Darker rust for secondary text
                    application.Resources["BorderColor"] = Color.FromArgb("#E0D5C1"); // Light amber/beige border
                }

                // Set success/warning/danger colors to be consistent with the palette
                application.Resources["SuccessColor"] = Color.FromArgb("#4D9E6A"); // Green with amber undertone
                application.Resources["WarningColor"] = Color.FromArgb("#F0B541"); // Brighter amber
                application.Resources["DangerColor"] = Color.FromArgb("#D83B01"); // Brighter rust
            });
        }
    }
}