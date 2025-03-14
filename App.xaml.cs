using Microsoft.Maui.Storage;
using TradiesToolbox.Views;

namespace TradiesToolbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Show SplashPage first
            MainPage = new SplashPage();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
