using ImageImporter.Application.Comparers;
using ImageImporter.Application.Hashing;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Enums;
using ImageImporter.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace ImageImporter.Application.Importers
{
    public class PictureImporter : BaseImporter<PictureImportResult>
    {
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        public PictureImporter(Settings settings, ApplicationDbContext context)
        {
            Settings = settings;
            Context = context;
        }

        protected override string ImportFolder => Settings.ImageImportFolder;

        protected override string ExportFolder => Settings.ImageExportFolder;

        protected override string RecylceFolder => Settings.ImageRecycleFolder;

        protected override bool UseRecycleFolder => Settings.ImageUseRecycleFolder;

        protected override async Task<bool> CheckFile(PictureImportResult result, string source)
        {
            IImageInfo srcInfo = Image.Identify(source);
            return srcInfo != null;
        }

        protected override async Task<byte[]?> CalculateHash(PictureImportResult result, string source)
        {
            IHashGenerator? hashGenerator = null;
            switch (Settings.ImageHashingAlgorithm)
            {
                case ImageHashingAlgorithms.AHashing:
                    hashGenerator = new ImageAHashGenerator();
                    break;
                case ImageHashingAlgorithms.DHashing:
                    hashGenerator = new ImageDHashGenerator();
                    break;
                case ImageHashingAlgorithms.PHashing:
                    hashGenerator = new ImagePHashGenerator();
                    break;
            }

            if (hashGenerator == null)
                return null;

            return await hashGenerator.Generate(source);
        }

        protected override async Task<(int Id, string File)?> FindExistingByHash(PictureImportResult result, byte[] hash)
        {
            (int Id, string File) ret;
            var match = Context.Pictures.FirstOrDefault(a => a.Hash.SequenceEqual(hash));
            if (match != null)
            {
                ret.Id = match.Id;
                ret.File = match.Path;
                return ret;
            }

            return null;
        }

        protected override async Task<bool?> CheckIfSourceIsBetter(PictureImportResult result, string source, string destination)
        {
            var src = new FileInfo(source);
            var dst = new FileInfo(destination);

            IQualityComparer? comparer = null;

            switch (Settings.ImageQualityCompareMethod)
            {
                case ImageQualityCompareMethods.Filesize:
                    comparer = new FileSizeComparer();
                    break;

                case ImageQualityCompareMethods.Resolution:
                    comparer = new ImageResolutionComparer();
                    break;
            }

            if (comparer == null)
                return null;

            return await comparer.CheckIfSourceIsBetter(source, destination);

        }

        protected override async Task AfterImport(PictureImportResult result)
        {
            if (!result.Success)
                return;

            Picture picture;
            switch (result.Status)
            {
                case ImportStatus.ImportedUniqueFile:
                    picture = new Picture();
                    picture.Path = result.RelativePath;
                    picture.Hash = result.Hash;
                    picture.Thumbnail = await CreateThumbnail(result, Path.Combine(ExportFolder, result.RelativePath));
                    Context.Add(picture);
                    break;

                case ImportStatus.HashMatchKeptSource:
                    picture = await Context.Pictures.FindAsync(result.Id);
                    picture.Thumbnail = await CreateThumbnail(result, Path.Combine(ExportFolder, result.RelativePath));
                    break;
            }
        }

        protected override async Task<string?> CreateThumbnail(PictureImportResult result, string file)
        {
            using var image = Image.Load<Rgba32>(file);
            image.Mutate(x => x.Resize(Settings.ImageThumbnailSize));
            var filename = DateTime.Now.ToString("yyyymmddHHmmssfff") + ".jpg";
            var path = Path.Combine(Settings.ImageThumbnailFolder, filename);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            await image.SaveAsJpegAsync(path);
            return filename;
        }

    }
}
