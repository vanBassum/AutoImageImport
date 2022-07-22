using ImageImporter.Data;
using ImageImporter.Models;
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
            //var db = from item in context.PictureImportItems
            //         orderby item.Id descending
            //         select new PictureImportItemViewModel
            //         {
            //             Id = item.Id,
            //             Source = item.Source,
            //             Destination = item.Destination,
            //             Hash = "0x" + Convert.ToHexString(BitConverter.GetBytes(item.Picture.Hash ?? 0)),
            //             File = Path.Combine(settings.ImageExportFolder, item.Picture.File).Replace("wwwroot", ""),
            //             Thumbnail = Path.Combine(settings.ImageThumbnailFolder, item.Picture.Thumbnail).Replace("wwwroot", ""),
            //             ActionType = ActionType.PictureImportItem,
            //         };


            var recycleFolder = settings.ImageRecycleFolder;
            var thumbnailFolder = settings.ImageThumbnailFolder;
            var exportFolder = settings.ImageExportFolder;


            var db = from a in context.PictureImportItems.Include(a => a.Picture)
                     orderby a.Id descending
                     select a;


            var model = new List<PictureImportItemViewModel>();

            foreach (var item in db.Take(100))
            {
                PictureImportItemViewModel piivm = new PictureImportItemViewModel();
                piivm.Id = item.Id;
                piivm.Source = item.Source;
                piivm.Destination = item.Destination;
                piivm.Hash = "0x" + Convert.ToHexString(BitConverter.GetBytes(item?.Picture?.Hash ?? 0));
                piivm.File = item?.Picture?.File?.Replace("wwwroot", "");
                piivm.Thumbnail = item?.Picture?.Thumbnail?.Replace("wwwroot", "");
                piivm.ActionType = ActionType.PictureImportItem;

                if (item is PictureMatchImportItem pmii)
                {
                    piivm.KeptSource = pmii.KeptSource;
                    piivm.RemovedFile = pmii.RemovedFile?.Replace("wwwroot", "");
                    piivm.RemovedFileThumbnail = pmii.RemovedFileThumbnail?.Replace("wwwroot", "");
                    piivm.ActionType = ActionType.PictureMatchImportItem;
                }
                model.Add(piivm);
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