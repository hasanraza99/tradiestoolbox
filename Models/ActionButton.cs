using System;
using System.Windows.Input;
using Microsoft.Maui.Graphics;

namespace TradiesToolbox.Models
{
    // Represents an interactive button used in the UI, often displayed in lists or actions
    public class ActionButton
    {
        public string Text { get; set; } // Button label
        public ICommand Command { get; set; } // Command triggered on click
        public Color BackgroundColor { get; set; } // Button background color
        public Color TextColor { get; set; } // Button text color
        public Color BorderColor { get; set; } // Button border color
        public int BorderWidth { get; set; } // Border thickness
    }
}