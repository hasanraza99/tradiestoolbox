﻿using TradiesToolbox.Views;
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
        /// Using Shell navigation instead of traditional push/pop navigation was more intuitive for me
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
            Routing.RegisterRoute(nameof(EditJobPage), typeof(EditJobPage));
            Routing.RegisterRoute(nameof(EditClientPage), typeof(EditClientPage));
        }

        /// <summary>
        /// Determines which page the app should start on.
        /// </summary>
        private void SetInitialPage()
        {
            try
            {
                var supabaseService = new SupabaseService();

                if (supabaseService.IsAuthenticated())
                {
                    // User is logged in
                    Console.WriteLine("User is logged in, navigating to Dashboard");
                    CurrentItem = new ShellContent { Content = new DashboardPage() };
                }
                else
                {
                    // User is not logged in
                    Console.WriteLine("User is not logged in, navigating to Login page");
                    CurrentItem = new ShellContent { Content = new LoginPage() };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetInitialPage: {ex.Message}");
                CurrentItem = new ShellContent { Content = new LoginPage() };
            }
        }
    }
}
