using SQLite;
using TradiesToolbox.Enums;

namespace TradiesToolbox.Models
{
    public class Quote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string QuoteNumber { get; set; }
        public string ClientName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public decimal Total { get; set; }

        // Store the enum as an integer for SQLite compatibility
        public int Status { get; set; }

        // Convert Status to Enum when getting/setting
        [Ignore]
        public QuoteStatus StatusEnum
        {
            get => (QuoteStatus)Status;
            set => Status = (int)value;
        }

        // Computed property for display purposes
        [Ignore]
        public string StatusDisplay => StatusEnum switch
        {
            QuoteStatus.Draft => "Draft",
            QuoteStatus.Sent => "Sent",
            QuoteStatus.Accepted => "Accepted",
            QuoteStatus.Rejected => "Rejected",
            QuoteStatus.Expired => "Expired",
            _ => "Unknown"
        };
    }
}
