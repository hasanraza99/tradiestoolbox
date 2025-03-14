using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    public partial class QuotesPage : ContentPage
    {
        private readonly QuotesViewModel _viewModel;

        public QuotesPage()
        {
            InitializeComponent();
            _viewModel = new QuotesViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadQuotesAsync();
        }
    }
}
