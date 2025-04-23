using System;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private readonly ClientDatabase _clientDatabase = new();
        public Client Client { get; set; } = new();

        private int _clientId;
        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                if (value > 0)
                {
                    // Load the client with this ID
                    LoadClient(value);
                }
            }
        }

        public ICommand SaveClientCommand { get; }
        public ICommand CancelCommand { get; }

        public EditClientViewModel()
        {
            Title = "Edit Client";
            SaveClientCommand = new Command(OnSaveClient);
            CancelCommand = new Command(OnCancel);
        }

        private void LoadClient(int clientId)
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Get client from database
                Client = _clientDatabase.GetClient(clientId);

                // Update title with client name
                Title = $"Edit: {Client.Name}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading client: {ex.Message}");
                Application.Current.MainPage.DisplayAlert("Error", $"Failed to load client: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnSaveClient()
        {
            if (string.IsNullOrWhiteSpace(Client.Name) || string.IsNullOrWhiteSpace(Client.Phone))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Client Name and Phone are required.", "OK");
                return;
            }

            try
            {
                _clientDatabase.UpdateClient(Client);
                await Shell.Current.GoToAsync(".."); // Navigate back after saving
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save client: {ex.Message}", "OK");
            }
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
    }
}