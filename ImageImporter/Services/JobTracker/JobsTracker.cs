using ImageImporter.Data;
using ImageImporter.Models.View;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Quartz.Impl.Matchers;
using System.Text.Json;

namespace ImageImporter.Services.JobTracker
{
    public class JobsTracker
    {
        private Dictionary<JobKey, JobInfo> Jobs { get; set; } = new();
        private readonly ISchedulerFactory factory;
        private readonly IHubContext<JobsHub> hubContext;

        public JobsTracker(ISchedulerFactory factory, IHubContext<JobsHub> hubContext)
        {
            this.factory = factory;
            this.hubContext = hubContext;
            PopulateJobsList().Wait();
        }


        private async Task PopulateJobsList()
        {
            var scheduler = await factory.GetScheduler();
            var allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());

            foreach (var triggerKey in allTriggerKeys)
            {
                var triggerdetails = await scheduler.GetTrigger(triggerKey);
                var jobKey = triggerdetails?.JobKey;

                if (jobKey != null)
                {
                    if (!Jobs.ContainsKey(jobKey))
                        Jobs.Add(jobKey, new JobInfo(jobKey));
                }
            }
        }


        private async Task UpdateFromQuartz(JobInfo info)
        {
            if (info.Key == null)
                return;

            //Fetch info from quartz
            var scheduler = await factory.GetScheduler();
            var triggers = await scheduler.GetTriggersOfJob(info.Key);
            var triggerDetails = triggers.First();
            if (triggerDetails != null)
            {
                //var jobdetails = await scheduler.GetJobDetail(triggerDetails.JobKey);
                info.NextExecution = triggerDetails.GetNextFireTimeUtc()?.ToLocalTime().UtcDateTime;
                info.LastExecution = triggerDetails.GetPreviousFireTimeUtc()?.ToLocalTime().UtcDateTime;
                info.Interval = info.NextExecution - info.LastExecution;
            }
        }



        public async Task ReportJobProgress(JobKey key, TimeSpan duration, float progress, string? progressMessage = null)
        {
            //Ensure jobinfo exists
            JobInfo info = Jobs[key];
            if (info == null)
                Jobs[key] = info = new JobInfo(key);

            //Fetch info from quartz
            await UpdateFromQuartz(info);

            info.Progress = progress;
            info.Duration = duration;

            if (progressMessage != null)
                info.ProgressMessage = progressMessage;

            string message = JsonSerializer.Serialize(new JobInfoViewModel( info));
            await hubContext.Clients.All.SendAsync("JobUpdate", message);
        }



        public JobKey? FindJobkeyByName(string name)
        {
            foreach(var kvp in Jobs)
            {
                if (kvp.Key.Name == name)
                    return kvp.Key;
            }
            return null;
        }



        public async Task<List<JobInfo>> GetJobsInfo()
        {
            List<JobInfo> result = new List<JobInfo>();

            foreach (var job in Jobs)
            {
                var info = job.Value;
                await UpdateFromQuartz(info);
                result.Add(info);
            }
            return result;
        }
    }
}
