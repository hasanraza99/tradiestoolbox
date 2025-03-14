using SQLite;
using TradiesToolbox.Enums;
using TradiesToolbox.Data; 

namespace TradiesToolbox.Models
{
    [Table("Jobs")]
    public class Job
    {
        [PrimaryKey, AutoIncrement]
        public int JobID { get; set; }

        public int ClientID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public DateTime ScheduledDate { get; set; }
        public TimeSpan Duration { get; set; }

        public int Status { get; set; }

        public decimal EstimatedCost { get; set; }
        public decimal ActualCost { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Notes { get; set; }

        // Computed Property - Converts Status from int to JobStatus Enum
        [Ignore]
        public JobStatus JobStatusEnum
        {
            get => (JobStatus)Status;
            set => Status = (int)value;
        }

        // Computed Property for Client Name
        [Ignore]
        public string ClientName { get; set; } // Now writable

        [Ignore]
        public string StatusDisplay => JobStatusEnum.ToString(); // Converts enum to string

        [Ignore]
        public string ScheduleDisplay => ScheduledDate.ToString("ddd, dd MMM yyyy");

        [Ignore]
        public string DurationDisplay => $"{Duration.Hours}h {Duration.Minutes}m";
    }
}
