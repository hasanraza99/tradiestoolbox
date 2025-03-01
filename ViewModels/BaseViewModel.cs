using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; // Contains INotifyPropertyChanged interface
using System.Runtime.CompilerServices; // Contains CallerMemberName attribute used in property change methods

// Using the namespace of the parent folder to avoid naming conflicts 
namespace TradiesToolbox.ViewModels
{
    /* This base class is used to implement INotifyPropertyChanged which is essential for XAML Data Binding
     * It allows the UI to update automatically when properties change
     * BaseViewModel inherits from INotifyPropertyChanged
     * Requires an event named PropertyChanged and logic to implement the event */
    public class BaseViewModel: INotifyPropertyChanged 
    {
        /* This event is defined by INotifyPropertyChanged and will be raised when a property changes
         * In simple terms, this class can notify others when a property changes by triggering this PropertyChanged event */
        public event PropertyChangedEventHandler PropertyChanged;

        /* This method actually raises the PropertyChanged event we defined in the previous line,
         * letting the UI know that a property value has changed.
         * CallerMemberName is an attribute. It tells the compiler to fill in propertyName with the name of the property that called this method. 
         * If one is not provided, it will default to null, as defined by string propertyName = null */
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            /* PropertyChanged? is the null conditional operator. It checks if PropertyChanged is null before invoking it.
             * .Invoke(...) is used if PropertyChanged is not null, and it calls all subscribed methods with the sender and event arguments in sequence
             * Each subscriber receives the same two arguments, 
             * this - the object that raised the event
             * new PropertyChangedEventArgs(propertyName) - information about which property changed
             * The main purpose is to notify any UI elements bound to this property that the value has changed, so they can update themselves */

            /* Think of it like a notification system:
             * The ViewModel says "Property X has changed!"
             * All UI elements that care about Property X need to update
             * The event system delivers this notification to every UI element that is subscribed to Property X 
             * Subscribed means a method has registered to receive notifications when an event occurs 
             * In .NET MAUI, the data binding system automatically sorts out subscription setup */
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            /* This method is used to set the value of a property and raise the PropertyChanged event if the value has changed
                 * ref T storage - This is a reference to the backing field of the property (a private field that stores the data exposed by a public property)
                 * T value - This is the new value we want to set the property to
                 * [CallerMemberName] string propertyName - This is the name of the property that called this method
                 * If the new value is different from the current value, the property is updated and the PropertyChanged event is raised
                 * The method returns true if the value has changed, otherwise it returns false */
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // This property is used to indicate whether the ViewModel is busy performing an operation
        // It is used to display a loading indicator in the UI
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }


        // This property is used to store the title of the page
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

    }
}