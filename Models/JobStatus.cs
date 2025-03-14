namespace TradiesToolbox.Models
{
    // Enum representing the different states a job can have
    public enum JobStatus
    {
        Quoted = 0,     // Job has been quoted but not confirmed
        Scheduled = 1,  // Job is scheduled but not yet started
        InProgress = 2, // Job is currently being worked on
        Completed = 3,  // Job is finished but not yet invoiced
        Invoiced = 4,   // Invoice has been generated
        Paid = 5,       // Invoice has been paid
        Canceled = 6    // Job has been canceled
    }
}
