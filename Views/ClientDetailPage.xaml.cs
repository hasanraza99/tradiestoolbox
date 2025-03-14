using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public partial class ClientDetailPage : ContentPage
    {
        private readonly ClientDetailViewModel _viewModel;

        private int _clientId;
        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                LoadClient(value); // Load client details as soon as clientId is set
            }
        }

        public ClientDetailPage()
        {
            InitializeComponent();
            _viewModel = new ClientDetailViewModel();
            BindingContext = _viewModel;
        }

        private async void LoadClient(int clientId)
        {
            await _viewModel.LoadClientAsync(clientId);
        }
    }
}
