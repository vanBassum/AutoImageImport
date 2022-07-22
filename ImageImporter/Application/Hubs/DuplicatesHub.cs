using ImageImporter.Data;
using ImageImporter.Services.Quartz;
using ImageImporter.Services.Quartz.JobTracker;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using System.Text.RegularExpressions;

namespace ImageImporter.Application.Hubs
{
    public class DuplicatesHub : Hub
    {
        private readonly ISchedulerFactory factory;
        private readonly JobsTracker tracker;
        private readonly ApplicationDbContext context;
        private readonly Settings settings;
        public DuplicatesHub(ISchedulerFactory factory,  JobsTracker tracker, ApplicationDbContext context, Settings settings)
        {
            this.factory = factory;
            this.tracker = tracker;
            this.context = context;
            this.settings = settings;
        }

        public async Task DeletePicture(string id)
        {
            if(int.TryParse(id, out int iid))
            {
                var picture = await context.Pictures.FindAsync(iid);

                if (picture != null)
                {
                    picture.Deleted = true;


                    string relPath = Path.GetRelativePath(settings.ImageExportFolder, picture.File);
                    string destination = Path.Combine(settings.ImageRecycleFolder, relPath);
                    destination = RenameUntillUnique(destination);
                    MoveFile(picture.File, destination);
                    picture.File = destination;

                    await context.SaveChangesAsync();
                    await Clients.All.SendAsync("Removed", id);
                }
            }
            
        }


        private void MoveFile(string source, string destination)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            File.Move(source, destination);
        }

        private static string RenameUntillUnique(string file)
        {
            string path = Path.GetDirectoryName(file);
            string fileName = Path.GetFileNameWithoutExtension(file);
            string extention = Path.GetExtension(file);
            Directory.CreateDirectory(path);    //Ensure path exists
            while (File.Exists(file))
            {
                var match = Regex.Match(fileName, @"(.+)\((\d+)\)$");
                if (match.Success)
                {
                    var number = int.Parse(match.Groups[2].Value);
                    number++;
                    fileName = $"{match.Groups[1].Value}({number.ToString()})";
                }
                else
                    fileName += "(1)";

                file = Path.Combine(path, fileName + extention);
            }
            return file;
        }

    }
}
