using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TradiesToolbox.ViewModels
{
    public class LoginViewModel:BaseViewModel
    {
        // Properties
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        // Commands
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            Title = "Login";

            // Initialise the commands
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }

        public void Initialize()
        {
            // Reset fields/perform initialisation
            Email = string.Empty;
            Password = string.Empty;
        }

        private async void OnLoginClicked()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                // Display an alert if the email or password is empty
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your email and password", "OK");
                return;
            }

            // For now, simulate a successful login
            if (Email.Contains("@") && Password.Length > 5)
            {
                // Navigate to the Home page
                await Application.Current.MainPage.DisplayAlert("Success", "Login successful", "OK");
            }

            else
            {
                // Display an alert if the login fails
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid email or password", "OK");
            }
        }

            private void OnRegisterClicked()
        {
            // For now, just show an alert
            Application.Current.MainPage.DisplayAlert("Register", "Redirecting to registration page", "OK");
        }
    }
}
