using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;
using TradiesToolbox.Views;

namespace TradiesToolbox.ViewModels
{
    public class QuotesViewModel : BaseViewModel
    {
        private readonly QuoteDatabase _quoteDatabase = new();

        private ObservableCollection<Quote> _quotes = new();
        public ObservableCollection<Quote> Quotes
        {
            get => _quotes;
            set => SetProperty(ref _quotes, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddQuoteCommand { get; }
        public ICommand QuoteSelectedCommand { get; }

        public QuotesViewModel()
        {
            Title = "Quotes";
            RefreshCommand = new Command(async () => await LoadQuotesAsync());
            AddQuoteCommand = new Command(OnAddQuote);
            QuoteSelectedCommand = new Command<Quote>(OnQuoteSelected);
            LoadQuotesAsync().ConfigureAwait(false);
        }

        public async Task LoadQuotesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var quoteList = _quoteDatabase.GetQuotes();
                Quotes = new ObservableCollection<Quote>(quoteList);
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

        private async void OnAddQuote()
        {
            await Shell.Current.GoToAsync(nameof(AddQuotePage));
        }

        private async void OnQuoteSelected(Quote quote)
        {
            if (quote == null) return;
            await Shell.Current.GoToAsync($"{nameof(QuoteDetailPage)}?quoteId={quote.Id}");
        }
    }
    
}