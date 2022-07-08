using ImageImporter.Helpers.Quartz;
using ImageImporter.Services;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System.Diagnostics;
using System.Text.Json;

namespace ImageImporter.Jobs
{
    [JobKey(nameof(ImportImagesJob))]
    [DisallowConcurrentExecution]
    public class ImportImagesJob : IJob
    {
        private readonly ILogger<ImportImagesJob> _logger;
        private readonly JobsTracker _jobsTracker;


        public ImportImagesJob(ILogger<ImportImagesJob> logger, JobsTracker jobsTracker)
        {
            _logger = logger;
            _jobsTracker = jobsTracker;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Stopwatch sw = Stopwatch.StartNew();
            int i = 0;
            while (i++ < 100)
            {
                await _jobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, i / 100.0f );
                Thread.Sleep(100);
            }
            sw.Stop();
            await _jobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, 1f);
        }
    }
}
