using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradiesToolbox.Models
{
    public static class NavigationHelper
    {
        private static bool _isNavigating = false;

        public static async Task SafeNavigateAsync(string route)
        {
            if (_isNavigating)
                return;

            try
            {
                _isNavigating = true;
                await Shell.Current.GoToAsync(route);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
            }
            finally
            {
                _isNavigating = false;
            }
        }
    }
}
