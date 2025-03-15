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
                var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

                // Remove any existing custom theme dictionaries
                var customThemeDicts = mergedDictionaries
                    .Where(dict => dict.ContainsKey("PrimaryColor"))
                    .ToList();

                foreach (var dict in customThemeDicts)
                {
                    mergedDictionaries.Remove(dict);
                }

                // Create a new theme dictionary
                var themeDict = new ResourceDictionary();

                if (IsDarkModeEnabled)
                {
                    // Dark theme colors
                    themeDict["PrimaryColor"] = Color.FromArgb("#6A8CAF");
                    themeDict["BackgroundColor"] = Color.FromArgb("#121212");
                    themeDict["TextColor"] = Colors.White;
                    themeDict["SecondaryTextColor"] = Color.FromArgb("#B0B0B0");
                }
                else
                {
                    // Light theme colors
                    themeDict["PrimaryColor"] = Color.FromArgb("#3E5C76");
                    themeDict["BackgroundColor"] = Colors.White;
                    themeDict["TextColor"] = Colors.Black;
                    themeDict["SecondaryTextColor"] = Color.FromArgb("#666666");
                }

                // Add the new theme dictionary
                mergedDictionaries.Add(themeDict);
            });
        }
    }
}