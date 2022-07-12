using ImageImporter.Helpers.Quartz;
using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace ImageImporter.Services.JobTracker
{
    public class JobsHub : Hub
    {
        private readonly ISchedulerFactory factory;
        private readonly JobsTracker tracker;
        public JobsHub(ISchedulerFactory factory, IHubContext<JobsHub> hubContext, JobsTracker tracker)
        {
            this.factory = factory;
            this.tracker = tracker; 
        }

        public async Task RunJob(string name)
        {
            var scheduler = await factory.GetScheduler();
            var jobKey = (await scheduler.GetJobKeyByName(name)).First();

            if(jobKey != null)
            {
                await scheduler.TriggerJob(jobKey);
            }
        }

    }
}
