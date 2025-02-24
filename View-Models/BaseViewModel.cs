using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; // Contains INotifyPropertyChanged interface
using System.Runtime.CompilerServices; // Contains CallerMemberName attribute used in property change methods

namespace TradiesToolbox.View_Models
{
    // This base class is used to implement INotifyPropertyChanged which is essential for XAML Data Binding
    // It allows the UI to update automatically when properties change
    public class BaseViewModel:INotifyPropertyChanged
    {
        // This method is used to notify the UI that a property has changed, it is defined by INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}