using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;
using TradiesToolbox.Views;

namespace TradiesToolbox.ViewModels
{
    public class ClientsViewModel : BaseViewModel
    {
        private readonly ClientDatabase _clientDatabase = new();

        private ObservableCollection<Client> _clients = new();
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterClients();
                }
            }
        }

        // Add missing command
        public ICommand SearchCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddClientCommand { get; }
        public ICommand ClientSelectedCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public ClientsViewModel()
        {
            Title = "Clients";
            RefreshCommand = new Command(async () => await LoadClientsAsync());
            AddClientCommand = new Command(OnAddClient);
            ClientSelectedCommand = new Command<Client>(OnClientSelected);
            DeleteClientCommand = new Command<Client>(OnDeleteClient);
            SearchCommand = new Command(() => FilterClients()); // Add this line

            LoadClientsAsync().ConfigureAwait(false);
        }

        private async Task LoadClientsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var clientList = _clientDatabase.GetClients();
                Clients = new ObservableCollection<Client>(clientList);
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

        private void FilterClients()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Clients = new ObservableCollection<Client>(_clientDatabase.GetClients());
            }
            else
            {
                Clients = new ObservableCollection<Client>(_clientDatabase.GetClients()
                    .Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                c.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                c.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private async void OnAddClient()
        {
            await Shell.Current.GoToAsync(nameof(AddClientPage));
        }

        private async void OnClientSelected(Client client)
        {
            if (client == null) return;
            await Shell.Current.GoToAsync($"{nameof(ClientDetailPage)}?clientId={client.Id}");
        }

        private async void OnDeleteClient(Client client)
        {
            if (client == null) return;
            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete", $"Delete {client.Name}?", "Yes", "No");
            if (confirm)
            {
                _clientDatabase.DeleteClient(client.Id);
                LoadClientsAsync().ConfigureAwait(false);
            }
        }
    }
}