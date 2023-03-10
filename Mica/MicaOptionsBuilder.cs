using Microsoft.Extensions.DependencyInjection;

namespace Mica
{
    public class MicaOptionsBuilder
    {
        IServiceCollection _services;
        public MicaOptionsBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public MicaOptionsBuilder AddJob<TJob>(Action<JobOptions> action = null) where TJob : class, IJob
        {
            _services.Configure<MicaOptions>(options => {
                var jobConfig = new JobConfiguration
                {
                    JobType = typeof(TJob),
                };

                if (action != null)
                {
                    var jobOptions = new JobOptions();
                    action?.Invoke(jobOptions);
                    jobConfig.Options = jobOptions;
                }
                options.Jobs.Add(jobConfig);
            });
            return this;
        }
    }
}
