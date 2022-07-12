using ImageImporter.Data;
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
        private readonly ApplicationDbContext context;
        private readonly Settings settings;

        public HomeController(ISchedulerFactory factory, ILogger<HomeController> logger, JobsTracker jobsTracker, ApplicationDbContext context, Settings settings)
        {
            this.factory = factory;
            this.logger = logger;
            this.jobsTracker = jobsTracker;
            this.context = context;
            this.settings = settings;
        }


        public async Task<IActionResult> Index()
        {
            IScheduler scheduler = await factory.GetScheduler();
            var allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());
            IEnumerable<JobInfoViewModel> model = (await jobsTracker.GetJobsInfo()).Select(a=>new JobInfoViewModel(a));
            return View(model);
        }

        public async Task<IActionResult> History()
        {
            IScheduler scheduler = await factory.GetScheduler();
            var allTriggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());

            List<ImportResultViewModel> model = new List<ImportResultViewModel>();
            foreach(var item in context.ImportResults.OrderByDescending(a => a.Id).Take(100).ToList())
            {
                var a = new ImportResultViewModel(item);
                if(item.MatchedWithId != null)
                {
                    a.RemovedThumb = Path.Combine(settings.ImageThumbnailFolder, item.RemovedFileThumb).Replace("wwwroot","");
                    a.Thumb = Path.Combine(settings.ImageThumbnailFolder, context.Pictures.Find(item.MatchedWithId)?.Thumbnail).Replace("wwwroot", "");
                }
                
                model.Add(a);
            }

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