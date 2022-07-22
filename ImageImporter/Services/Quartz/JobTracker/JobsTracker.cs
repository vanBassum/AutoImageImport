using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.View;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using Quartz.Impl.Matchers;
using System.Text.Json;

namespace ImageImporter.Services.Quartz.JobTracker
{
    public class JobsTracker : IJobListener
    {
        private Dictionary<JobKey, JobInfo> Jobs { get; set; } = new();

        public string Name => nameof(JobTracker);

        private readonly ISchedulerFactory factory;
        private readonly IHubContext<JobsHub> hubContext;

        public JobsTracker(ISchedulerFactory factory, IHubContext<JobsHub> hubContext)
        {
            this.factory = factory;
            this.hubContext = hubContext;
            Initialize().Wait();
        }


        private async Task Initialize()
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



        public async Task<List<JobInfo>> GetJobsInfo()
        {
            List<JobInfo> result = new List<JobInfo>();

            foreach (var job in Jobs)
            {
                var info = GetJobInfo(job.Key);
                await FixInfo(info);
                if (info != null)
                {
                    result.Add(info);
                }
            }
            return result;
        }

        public async Task ReportJobProgress(IJobExecutionContext context, float progress)
        {
            var info = GetJobInfo(context.JobDetail.Key);
            await FixInfo(info);
            info.Progress = progress;
            await ReportInfo(info);
        }

        public async Task ApplyJobStatistics(IJobExecutionContext context, JobResult result)
        {
            result.Started = context.FireTimeUtc.UtcDateTime;
            result.Duration = context.JobRunTime;
        }


        


        async Task ReportInfo(JobInfo info)
        {
            string message = JsonSerializer.Serialize(new JobInfoViewModel(info));
            await hubContext.Clients.All.SendAsync("JobUpdate", message);
        }

        async Task FixInfo(JobInfo info)
        {
            //Fetch info from quartz
            var scheduler = await factory.GetScheduler();
            var triggers = await scheduler.GetTriggersOfJob(info.Key);
            var triggerDetails = triggers.First();
            if (triggerDetails is ISimpleTrigger trg)
            {
                //var jobdetails = await scheduler.GetJobDetail(triggerDetails.JobKey);
                info.NextExecution = trg.GetNextFireTimeUtc()?.ToLocalTime().UtcDateTime;
                info.LastExecution = trg.GetPreviousFireTimeUtc()?.ToLocalTime().UtcDateTime;
                info.Interval = trg.RepeatInterval;
            }
        }




        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            var info = GetJobInfo(context.JobDetail.Key);
            info.Stopwatch.Start();
            await ReportJobProgress(context, 0f);
        }

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            var info = GetJobInfo(context.JobDetail.Key);
            info.Stopwatch.Stop();
            await ReportJobProgress(context, 0f);
        }



        JobInfo GetJobInfo(JobKey key)
        {
            //Ensure jobinfo exists
            JobInfo info = Jobs[key];
            if (info == null)
                Jobs[key] = info = new JobInfo(key);
            return info;
        }
    }
}
