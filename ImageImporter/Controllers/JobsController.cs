using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using Microsoft.AspNetCore.Authorization;
using ImageImporter.Services.Quartz.JobTracker;
using ImageImporter.Data;
using ImageImporter.Models.View;

namespace ImageImporter.Controllers
{
    public class JobsController : Controller
    {
        private readonly ISchedulerFactory factory;
        private readonly JobsTracker jobsTracker;
        private readonly ApplicationDbContext context;

        public JobsController(ISchedulerFactory factory, JobsTracker jobsTracker, ApplicationDbContext context)
        {
            this.factory = factory;
            this.jobsTracker = jobsTracker;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }



        public async Task<IActionResult> Jobs()
        {
            IEnumerable<JobInfoViewModel> model = (await jobsTracker.GetJobsInfo()).Select(a => new JobInfoViewModel(a));
            return PartialView("_Jobs", model);
        }


        public async Task<IActionResult> History()
        {
            //IScheduler scheduler = await factory.GetScheduler();
            //var allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            //IEnumerable<JobResultViewModel> model = context.JobResults.OrderByDescending(a => a.Id).Select(a => new JobResultViewModel(a)).Take(100);
            //return PartialView("_History", model);

            return PartialView("_History");
        }
    }
}
