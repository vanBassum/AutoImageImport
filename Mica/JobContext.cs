using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mica
{
    public class JobContext
    {
        public event EventHandler<double> OnProgressChanged;
        public event EventHandler OnCompleted;

        private static int idCounter = 0;
        public int Id { get; }
        public IJob Job { get; set; }
        public bool IsRunning => !Task?.IsCompleted ?? false;
        public Task? Task { get; set; }
        public double Progress { get; private set; }
        public DateTime Started { get; private set; }

        CancellationTokenSource cancellationTokenSource;
        Type jobType;
        IServiceProvider serviceProvider;
        public JobContext(Type jobType, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider; 
            this.jobType = jobType;
            Id = Interlocked.Increment(ref idCounter);
        }



        public void StartJob()
        {
            Started = DateTime.Now;
            cancellationTokenSource = new CancellationTokenSource();
            Progress<double> progress = new Progress<double>();
            progress.ProgressChanged += (s, e) =>
            {
                Progress = e;
                OnProgressChanged?.Invoke(this, Progress);
            };
            Task = Task.Run(async () =>
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    if (ActivatorUtilities.CreateInstance(scope.ServiceProvider, jobType) is IJob job)
                    {
                        Job = job;
                        await Job.Work(progress, cancellationTokenSource.Token);
                    }
                }
                catch(Exception ex)
                {

                }
                OnCompleted?.Invoke(this, EventArgs.Empty);
            });
        }

        public void CancelJob()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
