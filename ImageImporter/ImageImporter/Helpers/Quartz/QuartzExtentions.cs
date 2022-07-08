using Quartz;
using Quartz.Impl.Matchers;

namespace ImageImporter.Helpers.Quartz
{


    public static class QuartzExtentions
    {
        public static void AddJob<T>(this IServiceCollectionQuartzConfigurator quartz, Action<SimpleScheduleBuilder> action) where T : IJob
        {
            var keyAttribute = typeof(T).GetCustomAttributes<JobKeyAttribute>(true).First();
            string? key = keyAttribute?.Key;
            if (key == null)
                throw new Exception($"Missing Key attribute for {nameof(T)}");

            quartz.AddJob<T>(opts => opts.WithIdentity(key)) ;
            quartz.AddTrigger(opts => opts
                    .ForJob(key)
                    .WithIdentity($"{key}-trg")
                    .WithSimpleSchedule(x => action(x)));
        }

        public async static Task<List<JobKey>> GetJobKeyByName(this IScheduler scheduler, string name)
        {
            List<JobKey> result = new ();
            var allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());

            foreach (var triggerKey in allTriggerKeys)
            {
                var triggerdetails = await scheduler.GetTrigger(triggerKey);
                var jobKey = triggerdetails?.JobKey;

                if(jobKey.Name == name)
                {
                    result.Add(jobKey);
                }
            }
            return result;
        }

    }
}
