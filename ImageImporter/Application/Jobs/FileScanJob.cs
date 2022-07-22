using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using ImageImporter.Application.Comparers;
using ImageImporter.Application.Importers;
using ImageImporter.Application.Quantifiers;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Db.ActionItems;
using ImageImporter.Models.Enums;
using ImageImporter.Services.Quartz;
using ImageImporter.Services.Quartz.JobTracker;
using Quartz;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageImporter.Application.Jobs
{
    [JobKey(nameof(FileScanJob))]
    [DisallowConcurrentExecution]
    public class FileScanJob : IJob
    {
        private JobsTracker JobsTracker { get; }
        private ApplicationDbContext Context { get; }
        private Settings Settings { get; }

        public FileScanJob(JobsTracker jobsTracker, ApplicationDbContext context, Settings settings)
        {
            JobsTracker = jobsTracker;
            Context = context;
            Settings = settings;
        }

        public async Task Execute(IJobExecutionContext jobContext)
        {
            PictureImporter pictureImporter = new PictureImporter(Settings, Context);
            Directory.CreateDirectory(Settings.ImageExportFolder);
            var files = Directory.GetFiles(Settings.ImageExportFolder, "*", SearchOption.AllDirectories);
            int count = files.Length;
            JobResult jobResult = new JobResult();
            await JobsTracker.ApplyJobStatistics(jobContext, jobResult);
            Context.Add(jobResult);

            for (int i = 0; i < count; i++)
            {
                var file = files[i];
                //check if file is already in db
                var picture = Context.Pictures.FirstOrDefault(p=>p.File == file);

                if(picture == null)
                {
                    var hash = await CalculateHash(file);
                    var matches = Context.Pictures.Where(p => p.Hash == hash);
                    picture = await AddPictureToDb(file, hash.Value);
                }

                await JobsTracker.ApplyJobStatistics(jobContext, jobResult);
                await Context.SaveChangesAsync();
                await JobsTracker.ReportJobProgress(jobContext, i / (float)count);
            }
        }

        async Task<Picture> AddPictureToDb(string file, long hash)
        {
            var picture = new Picture();
            picture.File = file;
            picture.Hash = hash;
            picture.Thumbnail = await CreateThumbnail(file);
            picture.Quality = await QuantifyQuality(file);
            Context.Add(picture);

            PictureFoundItem item = new PictureFoundItem();
            item.Picture = picture;
            Context.Add(item);

            return picture;
        }



        private async Task<long?> CalculateHash(string source)
        {
            IImageHash? hashGenerator = null;
            switch (Settings.ImageHashingAlgorithm)
            {
                case ImageHashingAlgorithms.AHashing:
                    hashGenerator = new AverageHash();
                    break;
                case ImageHashingAlgorithms.DHashing:
                    hashGenerator = new DifferenceHash();
                    break;
                case ImageHashingAlgorithms.PHashing:
                    hashGenerator = new PerceptualHash();
                    break;
            }

            if (hashGenerator == null)
                return null;

            using var image = Image.Load<Rgba32>(source);
            return (long)hashGenerator.Hash(image);
        }

        private async Task<string?> CreateThumbnail(string file)
        {
            using var image = Image.Load<Rgba32>(file);
            image.Mutate(x => x.Resize(Settings.ImageThumbnailSize));
            var filename = DateTime.Now.ToString("yyyymmddHHmmssfff") + ".jpg";
            var path = Path.Combine(Settings.ImageThumbnailFolder, filename);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await image.SaveAsJpegAsync(path);
            return path;
        }

        public async Task<int> QuantifyQuality(string file)
        {
            var ImageQQ = new ImageResolutionQualityQuanifier();
            return await ImageQQ.DetermineQuality(file);
        }

    }
}
