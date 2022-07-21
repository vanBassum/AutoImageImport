using ImageImporter.Data;
using ImageImporter.Models;
using ImageImporter.Models.View;
using ImageImporter.Services.Quartz.JobTracker;
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
            return View();
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