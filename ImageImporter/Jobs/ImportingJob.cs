using ImageImporter.Application;
using ImageImporter.Application.Importers;
using ImageImporter.Data;
using ImageImporter.Helpers.Quartz;
using ImageImporter.Services;
using ImageImporter.Services.JobTracker;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System.Diagnostics;
using System.Text.Json;

namespace ImageImporter.Jobs
{
    [JobKey(nameof(ImportingJob))]
    [DisallowConcurrentExecution]
    public class ImportingJob : IJob
    {
        private ILogger<ImportingJob> Logger { get; }
        private JobsTracker JobsTracker { get; }
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        public ImportingJob(ILogger<ImportingJob> logger, JobsTracker jobsTracker, Settings settings, ApplicationDbContext context )
        {
            Logger = logger;
            JobsTracker = jobsTracker;
            Settings = settings;
            Context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, 0f, "Searching files");
            Directory.CreateDirectory(Settings.ImportFolder);
            var files = Directory.GetFiles(Settings.ImportFolder, "*", SearchOption.AllDirectories);
            int count = files.Length;
            FileImporter importer = new FileImporter(Settings, Context);
            for (int i=0; i<count; i++)
            {
                await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, (float)i / (float)count);
                var file = files[i];
                var result = await importer.ImportFile(new FileInfo(file));

                //TODO: Do something with this result!!!

                Thread.Sleep(100);
            }

            sw.Stop();
            await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, 0f, "Idle");
        }
    }
}
