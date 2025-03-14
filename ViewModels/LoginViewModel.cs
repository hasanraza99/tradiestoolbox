using System.Windows.Input;
using Microsoft.Maui.Controls;
using TradiesToolbox.Services;

namespace TradiesToolbox.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
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

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }

        public async void OnLoginClicked()
        {
            if (AuthService.Login(Email, Password))
            {
                await Shell.Current.GoToAsync($"//DashboardPage"); // Navigate to Dashboard after login
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid email or password", "OK");
            }
        }

        private async void OnRegisterClicked()
        {
            bool success = AuthService.Register(Email, Password);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Account created. You can now log in.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email already in use.", "OK");
            }
        }
    }
}
