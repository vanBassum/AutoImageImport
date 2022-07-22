using DB4045.Data;
using ImageImporter.Application.Importers;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Db.ActionItems;
using ImageImporter.Services.Quartz;
using ImageImporter.Services.Quartz.JobTracker;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ImageImporter.Application.Jobs
{
    [JobKey(nameof(FileImportJob))]
    [DisallowConcurrentExecution]
    public class FileImportJob : IJob
    {
        private JobsTracker JobsTracker { get; }
        private ApplicationDbContext Context { get; }
        private Settings Settings { get; }

        public FileImportJob(JobsTracker jobsTracker, ApplicationDbContext context, Settings settings)
        {
            JobsTracker = jobsTracker;
            Context = context;
            Settings = settings;
        }

        public async Task Execute(IJobExecutionContext jobContext)
        {
            PictureImporter pictureImporter = new PictureImporter(Settings, Context);

            Directory.CreateDirectory(Settings.ImageImportFolder);
            var files = Directory.GetFiles(Settings.ImageImportFolder, "*", SearchOption.AllDirectories);
            int count = files.Length;
            JobResult result = new JobResult();

            for (int i = 0; i < count; i++)
            {
                if(pictureImporter.CheckFile(files[i]))
                {
                    var item = new PictureImportItem();
                    await pictureImporter.TryImportFile(item, files[i]);
                    result.ActionsLog.Add(item);
                }

                await JobsTracker.ReportJobProgress(jobContext, i / (float)500f);
            }
            await JobsTracker.ApplyJobStatistics(jobContext, result);
            Context.Add(result);
            await Context.SaveChangesAsync();

        }
    }
}
