using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradiesToolbox.View_Models
{
    public class SplashViewModel:BaseViewModel
    {
        public SplashViewModel()
        {
            // Constructor
            Title = "Welcome";
        }

        public async Task Initialise()
        {
            await Task.Delay(2000); // 2 seconds

            await NavigateToLogin(); // Navigate to Login Page
        }

        private async Task NavigateToLogin()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.LoginPage())
        }
    }
}
