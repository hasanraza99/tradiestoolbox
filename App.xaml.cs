using Microsoft.Maui.Storage;
using TradiesToolbox.Views;
using TradiesToolbox.Data;

namespace TradiesToolbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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

            // Existing code
            var dbConnection = DBConnection.GetConnection();
            MainPage = new SplashPage();
        }
    }
}
