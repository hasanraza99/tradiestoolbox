using System.Threading.Tasks;
using TradiesToolbox.ViewModels;

namespace TradiesToolbox.Views
{
    // Connects XAML UI to the ViewModel logic
    public partial class SplashPage : ContentPage
    {
        // private field to store a reference to the ViewModel
        private SplashViewModel _viewModel;

        public SplashPage()
        {
            // Initialise the XAML components defined in SplashPage.xaml
            InitializeComponent();

            // Create a new instance of the SplashViewModel
            _viewModel = new SplashViewModel();

            /* Set the BindingContext of the page to the ViewModel
             * Enables data binding between the View and the ViewModel */
            BindingContext = _viewModel;
        }

        /* OnAppearing is a method that is called when the page is about to appear on the screen
         * Protected Override means we're extended the base class's method */
        protected override void OnAppearing()
        {
            // Always call the base implementation of the method first to ensure proper execution order
            base.OnAppearing();

            // start the initialisation without awaiting the result
            _viewModel.Initialize().ConfigureAwait(false);
        }
    }
}