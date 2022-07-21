using Quartz;
using System.Diagnostics;

namespace ImageImporter.Services.Quartz.JobTracker
{
    public class JobInfo
    {
        public JobKey? Key { get; set; }
        public TimeSpan? Interval { get; set; }
        public DateTime? LastExecution { get; set; }
        public DateTime? NextExecution { get; set; }
        public TimeSpan? Duration => Stopwatch.Elapsed;
        public bool IsRunning => Stopwatch.IsRunning;
        public float? Progress { get; set; }

        public Stopwatch Stopwatch { get; } = new Stopwatch();

        public JobInfo(JobKey key)
        {
            Key = key;
        }

    }
}
