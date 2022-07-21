using DB4045.Data;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Services.Quartz;
using ImageImporter.Services.Quartz.JobTracker;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ImageImporter.Application.Jobs
{
    [JobKey(nameof(DummyJob))]
    [DisallowConcurrentExecution]
    public class DummyJob : IJob
    {
        private JobsTracker JobsTracker { get; }
        private ApplicationDbContext Context { get; }

        public DummyJob(JobsTracker jobsTracker, ApplicationDbContext context)
        {
            JobsTracker = jobsTracker;
            Context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            for (int i = 0; i < 500; i++)
            {
                await JobsTracker.ReportJobProgress(context, i / (float)500f);
                Thread.Sleep(100);
            }
        }
    }
}
