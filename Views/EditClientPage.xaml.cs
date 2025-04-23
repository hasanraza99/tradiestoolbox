using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public partial class EditClientPage : ContentPage
    {
        private readonly EditClientViewModel _viewModel;

        // Property to receive client ID from navigation
        public int ClientId
        {
            set
            {
                if (_viewModel != null)
                {
                    // Pass the client ID to the ViewModel
                    _viewModel.ClientId = value;
                }
            }
        }

        public EditClientPage()
        {
            InitializeComponent();
            _viewModel = new EditClientViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}