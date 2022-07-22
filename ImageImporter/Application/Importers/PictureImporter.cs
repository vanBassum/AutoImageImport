using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using ImageImporter.Application.Comparers;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Db.ActionItems;
using ImageImporter.Models.Enums;
using ImageImporter.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ImageImporter.Application.Importers
{
    public class PictureImporter
    {
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        public PictureImporter(Settings settings, ApplicationDbContext context)
        {
            Settings = settings;
            Context = context;
        }



        public async Task<ActionItem?> TryImportFile(string source)
        {
            string relPath = Path.GetRelativePath(Settings.ImageImportFolder, source);
            var hash = await CalculateHash(source);
            string destination = Path.Combine(Settings.ImageExportFolder, relPath);
            var existingPicture = Context.Pictures.FirstOrDefault(a => a.Hash == hash);

            if (existingPicture == null)
            {
                destination = RenameUntillUnique(destination);
                MoveFile(source, destination);

                var picture = new Picture();
                picture.File = destination;
                picture.Hash = hash;
                picture.Thumbnail = await CreateThumbnail(destination);

                var result = new PictureImportItem();
                result.Source = source;
                result.Destination = destination;
                result.Picture = picture;
                return result;
            }
            else
            {
                var sourceIsBetter = await CheckIfSourceIsBetter(source, existingPicture.File);
                destination = existingPicture.File;
                if (sourceIsBetter.HasValue)
                {
                    var result = new PictureMatchImportItem();
                    result.Source = source;
                    result.Destination = destination;
                    result.KeptSource = sourceIsBetter.Value;
                    result.Picture = existingPicture;

                    if (sourceIsBetter.Value)
                    {
                        result.RemovedFile = await DeleteFile(Settings.ImageExportFolder, destination);
                        result.RemovedFileThumbnail = await CreateThumbnail(result.RemovedFile);
                        MoveFile(source, destination);
                    }
                    else
                    {
                        result.RemovedFile = await DeleteFile(Settings.ImageImportFolder, source);
                        result.RemovedFileThumbnail = await CreateThumbnail(result.RemovedFile);
                    }

                    return result;
                }
            }
            return null;
        }


        private async Task<string> DeleteFile(string fol, string file)
        {
            string relPath = Path.GetRelativePath(fol, file);
            string destination = Path.Combine(Settings.ImageRecycleFolder, relPath);
            destination = RenameUntillUnique(destination);
            MoveFile(file, destination);
            return destination;
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
            return (long) hashGenerator.Hash(image);
        }


        public bool CheckFile(string source)
        {
            IImageInfo srcInfo = Image.Identify(source);
            return srcInfo != null;
        }

        public static string RenameUntillUnique(string file)
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

        public void MoveFile(string source, string destination)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            File.Move(source, destination);
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


        private async Task<bool?> CheckIfSourceIsBetter(string source, string destination)
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
    }
}
