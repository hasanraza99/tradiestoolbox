namespace TradiesToolbox.Models
{
    // Represents a job entry in the dashboard's scheduled jobs list
    public class JobItem
    {
        public string Time { get; set; } // Scheduled time for the job (e.g., "9:00 AM")
        public string Title { get; set; } // Job description/title (e.g., "Kitchen Installation")
        public string Duration { get; set; } // Estimated job duration (e.g., "1.5 hrs")
        public string ClientInfo { get; set; } // Client's name and location (e.g., "John Smith - 123 Main St")
    }
}