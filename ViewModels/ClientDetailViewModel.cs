using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    [QueryProperty(nameof(ClientId), "id")]
    public class ClientDetailViewModel : BaseViewModel
    {
        private readonly ClientDatabase _clientDatabase = new();

        private Client _client;
        public Client Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        private int _clientId;
        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                LoadClientAsync(_clientId).ConfigureAwait(false);
            }
        }

        public ObservableCollection<ClientActivity> ClientActivity { get; } = new();
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CallCommand { get; }
        public ICommand EmailCommand { get; }
        public ICommand NewJobCommand { get; }
        public ICommand NewQuoteCommand { get; }

        public ClientDetailViewModel()
        {
            Title = "Client Details";
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);
            CallCommand = new Command(OnCall);
            EmailCommand = new Command(OnEmail);
            NewJobCommand = new Command(OnNewJob);
            NewQuoteCommand = new Command(OnNewQuote);
        }

        public async Task LoadClientAsync(int clientId)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Client = _clientDatabase.GetClient(clientId);
                Title = Client?.Name ?? "Client Details";
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnEdit()
        {
            await Application.Current.MainPage.DisplayAlert("Edit", "Navigate to Edit Client Page", "OK");
        }

        private async void OnDelete()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete Client", "Are you sure?", "Yes", "No");
            if (confirm)
            {
                _clientDatabase.DeleteClient(Client.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnCall()
        {
            await Application.Current.MainPage.DisplayAlert("Call", $"Calling {Client.Phone}...", "OK");
        }

        private async void OnEmail()
        {
            await Application.Current.MainPage.DisplayAlert("Email", $"Opening email for {Client.Email}...", "OK");
        }

        private async void OnNewJob()
        {
            await Application.Current.MainPage.DisplayAlert("New Job", "Navigate to New Job Page", "OK");
        }

        private async void OnNewQuote()
        {
            await Application.Current.MainPage.DisplayAlert("New Quote", "Navigate to New Quote Page", "OK");
        }
    }
}