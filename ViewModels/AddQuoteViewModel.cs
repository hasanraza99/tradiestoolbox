using System.Collections.ObjectModel;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using TradiesToolbox.Enums;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    public class AddQuoteViewModel : BaseViewModel
    {
        private readonly QuoteDatabase _quoteDatabase = new();
        private readonly ClientDatabase _clientDatabase = new();

        public Quote Quote { get; set; } = new();
        public ObservableCollection<Client> Clients { get; } = new();
        public ObservableCollection<QuoteStatus> Statuses { get; } = new();

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                Quote.ClientName = _selectedClient?.Name ?? string.Empty;
            }
        }

        private QuoteStatus _selectedStatus;
        public QuoteStatus SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                Quote.Status = (int)_selectedStatus;
            }
        }

        public ICommand SaveQuoteCommand { get; }
        public ICommand CancelCommand { get; }

        public AddQuoteViewModel()
        {
            Title = "Add Quote";
            SaveQuoteCommand = new Command(OnSaveQuote);
            CancelCommand = new Command(OnCancel);

            LoadClients();
            LoadStatuses();
        }

        private void LoadClients()
        {
            var clientList = _clientDatabase.GetClients();
            foreach (var client in clientList)
                Clients.Add(client);
        }

        private void LoadStatuses()
        {
            foreach (var status in (QuoteStatus[])Enum.GetValues(typeof(QuoteStatus)))
                Statuses.Add(status);
        }

        private async void OnSaveQuote()
        {
            if (string.IsNullOrWhiteSpace(Quote.QuoteNumber) || SelectedClient == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Quote Number and Client are required.", "OK");
                return;
            }

            _quoteDatabase.AddQuote(Quote);
            await Shell.Current.GoToAsync(".."); // Navigate back after saving
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
    }
}
