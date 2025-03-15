using System.Windows.Input;
using Microsoft.Maui.Controls;
using TradiesToolbox.Services;

namespace TradiesToolbox.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly SupabaseService _supabaseService;

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
            _supabaseService = new SupabaseService();
            LoginCommand = new Command(async () => await OnLoginClicked());
            RegisterCommand = new Command(async () => await OnRegisterClicked());
        }

        private async Task OnLoginClicked()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                var session = await _supabaseService.SignInAsync(Email, Password);

                if (session != null)
                {
                    // Login successful
                    Application.Current.MainPage = new AppShell();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnRegisterClicked()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both email and password", "OK");
                return;
            }

            try
            {
                IsBusy = true;
                var session = await _supabaseService.SignUpAsync(Email, Password);

                if (session != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Account created. You can now log in.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Registration Failed", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
