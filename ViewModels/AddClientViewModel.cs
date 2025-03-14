using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    public class AddClientViewModel : BaseViewModel
    {
        private readonly ClientDatabase _clientDatabase = new();
        public Client Client { get; set; } = new();

        public ICommand SaveClientCommand { get; }
        public ICommand CancelCommand { get; }

        public AddClientViewModel()
        {
            Title = "Add Client";
            SaveClientCommand = new Command(OnSaveClient);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnSaveClient()
        {
            if (string.IsNullOrWhiteSpace(Client.Name) || string.IsNullOrWhiteSpace(Client.Phone))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Client Name and Phone are required.", "OK");
                return;
            }

            _clientDatabase.AddClient(Client);
            await Shell.Current.GoToAsync(".."); // Navigate back after saving
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
    }
}
