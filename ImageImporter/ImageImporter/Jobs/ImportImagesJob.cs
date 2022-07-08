using ImageImporter.Helpers.Quartz;
using Microsoft.AspNetCore.SignalR;
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


    public class ConconcurrentJob : IJob
    {
        private readonly ILogger<ConconcurrentJob> _logger;
        private static int _counter = 0;
        private readonly IHubContext<JobsHub> _hubContext;

        public ConconcurrentJob(ILogger<ConconcurrentJob> logger,
            IHubContext<JobsHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var count = _counter++;

            var beginMessage = $"Conconcurrent Job BEGIN {count} {DateTime.UtcNow}";
            await _hubContext.Clients.All.SendAsync("ConcurrentJobs", beginMessage);
            _logger.LogInformation(beginMessage);

            Thread.Sleep(7000);

            var endMessage = $"Conconcurrent Job END {count} {DateTime.UtcNow}";
            await _hubContext.Clients.All.SendAsync("ConcurrentJobs", endMessage);
            _logger.LogInformation(endMessage);
        }
    }

    [DisallowConcurrentExecution]
    public class NonConconcurrentJob : IJob
    {
        private readonly ILogger<NonConconcurrentJob> _logger;
        private static int _counter = 0;
        private readonly IHubContext<JobsHub> _hubContext;

        public NonConconcurrentJob(ILogger<NonConconcurrentJob> logger,
               IHubContext<JobsHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var count = _counter++;

            var beginMessage = $"NonConconcurrentJob Job BEGIN {count} {DateTime.UtcNow}";
            await _hubContext.Clients.All.SendAsync("NonConcurrentJobs", beginMessage);
            _logger.LogInformation(beginMessage);

            Thread.Sleep(7000);

            var endMessage = $"NonConconcurrentJob Job END {count} {DateTime.UtcNow}";
            await _hubContext.Clients.All.SendAsync("NonConcurrentJobs", endMessage);
            _logger.LogInformation(endMessage);
        }
    }

    public class JobsHub : Hub
    {
        public Task SendConcurrentJobsMessage(string message)
        {
            return Clients.All.SendAsync("ConcurrentJobs", message);
        }

        public Task SendNonConcurrentJobsMessage(string message)
        {
            return Clients.All.SendAsync("NonConcurrentJobs", message);
        }

    }


}
