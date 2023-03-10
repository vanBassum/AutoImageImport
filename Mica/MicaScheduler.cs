using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace Mica
{

    public class MicaScheduler
    {
        List<SimpleTrigger> triggers = new List<SimpleTrigger>();
        System.Timers.Timer timer;
        MicaOptions _options;
        List<JobContext> jobContexts = new List<JobContext>();
        IServiceProvider _serviceProvider;
        public MicaScheduler(IOptions<MicaOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _options = options.Value;



            foreach (var jobConfig in _options.Jobs)
            {
                foreach (var trigger in jobConfig.Options.Triggers)
                {
                    SimpleTrigger trg = new SimpleTrigger();
                    trg.Interval = trigger.Options.Interval;
                    trg.JobType = jobConfig.JobType;
                    triggers.Add(trg);
                }
            }

            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            foreach (var trigger in triggers)
            {
                if (trigger.GetTriggerMoment() <= DateTime.Now)
                {
                    var contexts = jobContexts.Where(a => a.Job.GetType() == trigger.JobType).ToArray();
                    bool stillRunning = false;
                    foreach (var ctx in contexts)
                    {
                        if (ctx.IsRunning)
                            stillRunning = true;
                        else
                        {
                            //Cleanup context
                            jobContexts.Remove(ctx);
                        }
                    }

                    JobConcurrencyAttribute? attribute = trigger.JobType.GetCustomAttribute<JobConcurrencyAttribute>();
                    bool allowConcurrent = attribute?.AllowConcurrent ?? false;

                    if (!stillRunning || allowConcurrent)
                    {
                        var context = StartJob(trigger.JobType);
                        jobContexts.Add(context);
                        trigger.Triggered();
                    }
                }
            }
            timer.Start();
        }

        private JobContext StartJob(Type jobType)
        {
            JobContext context = new JobContext(jobType, _serviceProvider);
            context.StartJob();
            return context;
        }
    }
}
