using ImageImporter.Models;
using ImageImporter.Models.View;
using ImageImporter.Services;
using ImageImporter.Services.JobTracker;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Matchers;
using System.Diagnostics;

namespace ImageImporter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ISchedulerFactory factory;
        private readonly JobsTracker jobsTracker;

        public HomeController(ISchedulerFactory factory, ILogger<HomeController> logger, JobsTracker jobsTracker)
        {
            this.factory = factory;
            this.logger = logger;
            this.jobsTracker = jobsTracker;
        }


        public async Task<IActionResult> Index()
        {
            IScheduler scheduler = await factory.GetScheduler();
            var allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            IEnumerable<JobInfoViewModel> model = (await jobsTracker.GetJobsInfo()).Select(a=>new JobInfoViewModel(a));
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}