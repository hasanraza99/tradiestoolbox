using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    [QueryProperty(nameof(QuoteId), "quoteId")]
    public partial class QuoteDetailPage : ContentPage
    {
        private readonly QuoteDetailViewModel _viewModel;

        private int _quoteId;
        public int QuoteId
        {
            get => _quoteId;
            set
            {
                _quoteId = value;
                LoadQuote(value); // Load quote details as soon as quoteId is set
            }
        }

        public QuoteDetailPage()
        {
            InitializeComponent();
            _viewModel = new QuoteDetailViewModel();
            BindingContext = _viewModel;
        }

        private async void LoadQuote(int quoteId)
        {
            await _viewModel.LoadQuoteAsync(quoteId);
        }
    }
}
