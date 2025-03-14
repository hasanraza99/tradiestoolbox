using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    public partial class AddClientPage : ContentPage
    {
        private readonly AddClientViewModel _viewModel;

        public AddClientPage()
        {
            InitializeComponent();
            _viewModel = new AddClientViewModel();
            BindingContext = _viewModel;
        }
    }
}
