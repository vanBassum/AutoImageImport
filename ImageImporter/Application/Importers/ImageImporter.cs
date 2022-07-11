using ImageImporter.Application.Comparers;
using ImageImporter.Application.Hashing;
using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Enums;
using ImageImporter.Services;
using SixLabors.ImageSharp;
using System.Text.RegularExpressions;

namespace ImageImporter.Application.Importers
{

    public class ImageImporter : BaseImporter
    {
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        public ImageImporter(Settings settings, ApplicationDbContext context)
        {
            Settings = settings;
            Context = context;
        }

        protected override string ImportFolder => Settings.ImageImportFolder;

        protected override string ExportFolder => Settings.ImageExportFolder;

        protected override string RecylceFolder => Settings.ImageRecycleFolder;

        protected override bool UseRecycleFolder => Settings.ImageUseRecycleFolder;

        protected override async Task<bool> CheckFile(string source)
        {
            IImageInfo srcInfo = Image.Identify(source);
            return srcInfo != null;
        }

        protected override async Task<byte[]?> CalculateHash(string source)
        {
            IHashGenerator? hashGenerator = null;
            switch (Settings.ImageHashingAlgorithm)
            {
                case ImageHashingAlgorithms.AHashing:
                    hashGenerator = new ImageAHashGenerator()
                    {
                        HashWidth = Settings.ImageHashWidth,
                        HashHeight = Settings.ImageHashHeight,
                    };
                    break;
                case ImageHashingAlgorithms.DHashing:
                    hashGenerator = new ImageDHashGenerator()
                    {
                        HashWidth = Settings.ImageHashWidth,
                        HashHeight = Settings.ImageHashHeight,
                    };
                    break;
            }

            if (hashGenerator == null)
                return null;

            return await hashGenerator.Generate(source);
        }

        protected override async Task<string?> FindExistingByHash(byte[] hash)
        {
            var match = Context.Pictures.FirstOrDefault(a => a.Hash.SequenceEqual(hash));
            return match?.Path;
        }

        protected override async Task<bool?> CheckIfSourceIsBetter(string source, string destination)
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

        protected override async Task<ImportResult> AfterImport(ImportResult import)
        {
            if (!import.Success)
                return import;

            switch(import.Status)
            {
                case ImportStatus.ImportedUniqueFile:
                    Picture picture = new Picture();
                    picture.Path = import.RelativePath;
                    picture.Hash = import.Hash;
                    Context.Add(picture);
                    break;

                case ImportStatus.HashMatchKeptSource:
                    break;
            }

            return import;
        }

    }
}
