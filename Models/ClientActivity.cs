namespace TradiesToolbox.Models
{
    // Represents an activity associated with a client, such as job completions or payments
    public class ClientActivity
    {
        public string Description { get; set; } // Description of the activity
        public DateTime Date { get; set; } // Date the activity occurred
        public string Color { get; set; } // Activity status color
        public string Type { get; set; } // Type of activity (Job, Quote, Invoice, etc.)
    }
}