namespace TradiesToolbox.Models
{
    // Represents a recent activity item displayed in the dashboard
    public class ActivityItem
    {
        public string Description { get; set; } // Short activity description (e.g., "Quote #1052 was accepted")
        public string Timestamp { get; set; } // Time since the event occurred (e.g., "Today", "Yesterday")
        public string Color { get; set; } // Visual indicator color (e.g., Red for failed, Green for success)
    }
}