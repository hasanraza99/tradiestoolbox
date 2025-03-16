using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    [QueryProperty(nameof(ClientId), "clientId")]
    public partial class AddJobPage : ContentPage
    {
        private readonly AddJobViewModel _viewModel;

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

        public AddJobPage()
        {
            InitializeComponent();
            _viewModel = new AddJobViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // could refresh any data here if needed
        }
    }
}