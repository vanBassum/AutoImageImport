using ImageImporter.Services;
using ImageImporter.Services.JobTracker;

namespace ImageImporter.Models.View
{
    public class JobInfoViewModel
    {
        public string Name { get; set; }
        public string Interval { get; set; }
        public string LastExecution { get; set; }
        public string Duration { get; set; }
        public string NextExecution { get; set; }
        public string Progress { get; set; }
        public string ProgressMessage { get; set; }
        public bool IsRunning { get; set; }

        public JobInfoViewModel(JobInfo info)
        {
            Name = info.Key.Name;
            LastExecution = info.LastExecution?.ToString(@"dd-MM-yyyy HH:mm:ss");
            NextExecution = info.NextExecution?.ToString(@"dd-MM-yyyy HH:mm:ss");
            Interval = info.Interval?.ToString(@"hh\:mm\:ss");
            Duration = info.Duration?.ToString(@"hh\:mm\:ss");
            Progress = (info.Progress*100)?.ToString("F0");
            ProgressMessage = info.ProgressMessage;
            IsRunning = info.IsRunning;
        }

    }
}
