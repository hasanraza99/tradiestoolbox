using System.Threading.Tasks;
using System.Windows.Input;
using TradiesToolbox.Models;
using TradiesToolbox.Data;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    [QueryProperty(nameof(QuoteId), "id")]
    public class QuoteDetailViewModel : BaseViewModel
    {
        private readonly QuoteDatabase _quoteDatabase = new();

        private Quote _quote;
        public Quote Quote
        {
            get => _quote;
            set => SetProperty(ref _quote, value);
        }

        private int _quoteId;
        public int QuoteId
        {
            get => _quoteId;
            set
            {
                _quoteId = value;
                LoadQuoteAsync(_quoteId).ConfigureAwait(false);
            }
        }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public QuoteDetailViewModel()
        {
            Title = "Quote Details";
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);
        }

        public async Task LoadQuoteAsync(int quoteId)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                Quote = _quoteDatabase.GetQuote(quoteId);
                Title = Quote?.QuoteNumber ?? "Quote Details";
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

        private async void OnEdit()
        {
            await Application.Current.MainPage.DisplayAlert("Edit", "Navigate to Edit Quote Page", "OK");
        }

        private async void OnDelete()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete Quote", "Are you sure?", "Yes", "No");
            if (confirm)
            {
                _quoteDatabase.DeleteQuote(Quote.Id); // Changed Quote.QuoteID to Quote.Id
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}