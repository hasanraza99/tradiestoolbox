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

            // Initialize global exception handling
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = args.ExceptionObject as Exception;
                Console.WriteLine($"Unhandled exception: {exception?.Message}");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Console.WriteLine($"Unobserved task exception: {args.Exception.Message}");
                args.SetObserved(); // Prevent app crash
            };

            // Apply theme on startup
            ThemeService.ApplyTheme();

            // Existing code
            var dbConnection = DBConnection.GetConnection();
            MainPage = new SplashPage();
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
    }
}