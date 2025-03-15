using Microsoft.Maui.Storage;
using TradiesToolbox.Views;
using TradiesToolbox.Data;

namespace TradiesToolbox
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Ensure database is initialized
            var dbConnection = DBConnection.GetConnection();

            // Show SplashPage first
            MainPage = new SplashPage();
        }
    }
}
