using TradiesToolbox.Views;
using TradiesToolbox.Services;
using Microsoft.Maui.Storage;

namespace TradiesToolbox
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register navigation routes
            RegisterRoutes();

            // Determine the start page based on authentication status
            SetInitialPage();
        }

        /// <summary>
        /// Registers routes for navigation to different pages.
        /// </summary>
        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(ClientDetailPage), typeof(ClientDetailPage));
            Routing.RegisterRoute(nameof(JobDetailPage), typeof(JobDetailPage));
            Routing.RegisterRoute(nameof(QuoteDetailPage), typeof(QuoteDetailPage));
            Routing.RegisterRoute(nameof(AddJobPage), typeof(AddJobPage));
            Routing.RegisterRoute(nameof(AddClientPage), typeof(AddClientPage));
            Routing.RegisterRoute(nameof(AddQuotePage), typeof(AddQuotePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage)); // Added login page
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
        }

        /// <summary>
        /// Determines which page the app should start on.
        /// </summary>
        private void SetInitialPage()
        {
            if (AuthService.IsLoggedIn())
            {
                // User is logged in, go to the dashboard
                CurrentItem = new ShellContent { Content = new DashboardPage() };
            }
            else
            {
                // User is not logged in, navigate to login page
                CurrentItem = new ShellContent { Content = new LoginPage() };
            }
        }
    }
}
