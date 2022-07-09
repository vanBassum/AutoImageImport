using Quartz;

namespace ImageImporter.Services.JobTracker
{
    public class JobInfo
    {
        public JobKey? Key { get; set; }
        public TimeSpan? Interval { get; set; }
        public DateTime? LastExecution { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? NextExecution { get; set; }
        public float? Progress { get; set; }
        public string ProgressMessage { get; set; } = "Idle";
        public bool IsRunning { get; set; }

        public JobInfo(JobKey key)
        {
            Key = key;
        }

    }
}
