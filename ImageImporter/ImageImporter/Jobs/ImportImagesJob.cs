using ImageImporter.Helpers.Quartz;
using Quartz;

namespace ImageImporter.Jobs
{
    [JobKey(nameof(ImportImagesJob))]
    [DisallowConcurrentExecution]
    public class ImportImagesJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            int i = 0;
            while (i++ < 100)
            {
                context.JobDetail.JobDataMap["progress"] = i;
                Thread.Sleep(1000);
            }
            return Task.CompletedTask;
        }
    }

}
