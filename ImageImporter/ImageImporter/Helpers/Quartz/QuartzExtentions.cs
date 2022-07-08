using Quartz;

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

            quartz.AddJob<T>(opts => opts.WithIdentity(key));
            quartz.AddTrigger(opts => opts
                    .ForJob(key)
                    .WithIdentity($"{key}-trg")
                    .WithSimpleSchedule(x => action(x)));
        }
    }
}
