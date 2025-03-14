using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace TradiesToolbox.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        public SplashViewModel()
        {
            Title = "Welcome";
        }

        public async Task Initialize()
        {
            await Task.Delay(2000);
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
