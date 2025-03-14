using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    public partial class AddJobPage : ContentPage
    {
        private readonly AddJobViewModel _viewModel;

        public AddJobPage()
        {
            InitializeComponent();
            _viewModel = new AddJobViewModel();
            BindingContext = _viewModel;
        }
    }
}
