using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    public partial class AddQuotePage : ContentPage
    {
        private readonly AddQuoteViewModel _viewModel;

        public AddQuotePage()
        {
            InitializeComponent();
            _viewModel = new AddQuoteViewModel();
            BindingContext = _viewModel;
        }
    }
}
