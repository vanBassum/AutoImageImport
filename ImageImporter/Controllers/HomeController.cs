using ImageImporter.Data;
using ImageImporter.Models;
using ImageImporter.Models.Db;
using ImageImporter.Models.Db.ActionItems;
using ImageImporter.Models.Enums;
using ImageImporter.Models.View;
using ImageImporter.Services.Quartz.JobTracker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz;
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
            var actions = context.ActionItems.OrderByDescending(a => a.Id).Take(100).ToList();
            return View(actions);
        }


        public async Task<IActionResult> Duplicates()
        {

            var duplicates = context.Pictures.Where(p=>!p.Deleted).OrderBy(p => p.Hash).Take(100).ToList().NonDistinct(a=>a.Hash);

            return View(duplicates);


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

    public static class Ext
    {
        public static IEnumerable<T> NonDistinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Where(g => g.Count() > 1).SelectMany(r => r);
        }
    }

}