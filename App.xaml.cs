using Microsoft.Maui.Storage;
using TradiesToolbox.Views;
using TradiesToolbox.Data;
using TradiesToolbox.Services;

namespace TradiesToolbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Initialize global exception handling for better error reporting
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = args.ExceptionObject as Exception;
                LogError($"Unhandled exception: {exception?.Message}", exception);
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                LogError($"Unobserved task exception: {args.Exception.Message}", args.Exception);
                args.SetObserved(); // Prevent app crash
            };

            // Apply theme on startup
            ThemeService.ApplyTheme();

            // Initialize database connection
            try
            {
                var dbConnection = DBConnection.GetConnection();
                MainPage = new SplashPage();
            }
            catch (Exception ex)
            {
                LogError($"Database initialization error: {ex.Message}", ex);
                // Show a user-friendly error page instead of crashing
                MainPage = new ContentPage
                {
                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Label
                            {
                                Text = "Unable to start the application. Please restart.",
                                HorizontalOptions = LayoutOptions.Center
                            },
                            new Button
                            {
                                Text = "Try Again",
                                Command = new Command(() => MainPage = new SplashPage())
                            }
                        }
                    }
                };
            }
        }

        protected override void OnStart()
        {
            // Apply theme again when app starts
            ThemeService.ApplyTheme();
        }

        protected override void OnResume()
        {
            // Reapply theme when app resumes
            ThemeService.ApplyTheme();
        }

        /// <summary>
        /// Central method for logging errors
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            Console.WriteLine(message);

            if (ex != null)
            {
                Console.WriteLine($"Exception type: {ex.GetType().Name}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}