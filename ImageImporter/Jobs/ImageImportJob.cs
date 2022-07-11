using ImageImporter.Application;
using ImageImporter.Application.Importers;
using ImageImporter.Data;
using ImageImporter.Helpers.Quartz;
using ImageImporter.Models.Db;
using ImageImporter.Services;
using ImageImporter.Services.JobTracker;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System.Diagnostics;
using System.Text.Json;

namespace ImageImporter.Jobs
{
    [JobKey(nameof(ImageImportJob))]
    [DisallowConcurrentExecution]
    public class ImageImportJob : IJob
    {
        private ILogger<ImageImportJob> Logger { get; }
        private JobsTracker JobsTracker { get; }
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        private static string[] IgnoreExtentions => new string[] { ".db", ".db@SynoEAStream" };

        public ImageImportJob(ILogger<ImageImportJob> logger, JobsTracker jobsTracker, Settings settings, ApplicationDbContext context )
        {
            Logger = logger;
            JobsTracker = jobsTracker;
            Settings = settings;
            Context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Stopwatch sw = Stopwatch.StartNew();
            DateTime jobStartTime = DateTime.Now;
            await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, 0f, "Searching files");
            Directory.CreateDirectory(Settings.ImageImportFolder);
            var files = Directory.GetFiles(Settings.ImageImportFolder, "*", SearchOption.AllDirectories);
            int count = files.Length;
            FileImporter importer = new FileImporter(Settings, Context);
            await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, 0f, "Importing files");
            for (int i=0; i<count; i++)
            {
                var extention = Path.GetExtension(files[i]);
                if(!IgnoreExtentions.Contains(extention))
                {
                    await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, (float)i / (float)count);
                    ImportResult result = new ImportResult();
                    result.JobStartTime = jobStartTime;
                    await importer.ImportFile(files[i], result);
                    Context.Add(result);
                    try
                    {
                        await Context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }    
            }

            sw.Stop();
            await JobsTracker.ReportJobProgress(context.JobDetail.Key, sw.Elapsed, 0f, "Idle");
        }
    }
}
