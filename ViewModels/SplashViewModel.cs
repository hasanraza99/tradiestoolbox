using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradiesToolbox.ViewModels;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    // SplashViewModel inherits from BaseViewModel to get property change notification functionality
    public class SplashViewModel:BaseViewModel
    {
        public SplashViewModel()
        {
            /* Constructor - initialisation code goes here
             * Set the Title property to "Welcome" */
            Title = "Welcome";
        }

        /* This method will be called when the splash page appears
         * It's async because it uses await to create a non-blocking delay */
        public async Task Initialise()
        {
            /* The splash screen needs to be visible for a short time before navigating to the login page
             * This delay is achieved using Task.Delay() which creates a non-blocking delay
             * 2000 milliseconds = 2 seconds delay */
            await Task.Delay(2000);

            /* After the delay, navigate to the login page 
             * Async call, so we use await to ensure proper execution order */
            await NavigateToLogin(); 
        }

        // Private helper method to navigate to the login page
        // Async because navigation in MAUI is asynchronous
        private async Task NavigateToLogin()
        {
            // Push the login page onto the navigation stack
            // await Application.Current.MainPage.Navigation.PushAsync(new Views.LoginPage())

            Console.WriteLine("Navigate to login page");
        }
    }
}
