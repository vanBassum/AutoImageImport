using ImageImporter.Application.Importers;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Db.ActionItems;
using ImageImporter.Services.Quartz;
using ImageImporter.Services.Quartz.JobTracker;
using Quartz;

namespace ImageImporter.Application.Jobs
{
    [JobKey(nameof(FileCleanupJob))]
    [DisallowConcurrentExecution]
    public class FileCleanupJob : IJob
    {
        private JobsTracker JobsTracker { get; }
        private ApplicationDbContext Context { get; }
        private Settings Settings { get; }

        public FileCleanupJob(JobsTracker jobsTracker, ApplicationDbContext context, Settings settings)
        {
            JobsTracker = jobsTracker;
            Context = context;
            Settings = settings;
        }

        public async Task Execute(IJobExecutionContext jobContext)
        {
            PictureImporter pictureImporter = new PictureImporter(Settings, Context);
            JobResult jobResult = new JobResult();
            Context.Add(jobResult);

            int counter = 0;
            var list = Context.Pictures.Where(p => p.Deleted == false).ToList();

            foreach(var picture in list)
            {
                if (!File.Exists(picture.File))
                {
                    picture.Deleted = true;

                    PictureRemovedItem item = new PictureRemovedItem();
                    item.Picture = picture;
                    Context.Add(item);
                }


                await JobsTracker.ApplyJobStatistics(jobContext, jobResult);
                await Context.SaveChangesAsync();
                counter++;
                await JobsTracker.ReportJobProgress(jobContext, counter / (float)list.Count);
                
            }
        }
    }
}
