using ImageImporter.Models.Enums;
using ImageImporter.Services;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Data
{
    public class Settings : SettingsService
    {
        public Settings(ApplicationDbContext context) : base(context)
        {
        }

        public string ImageImportFolder { get => GetPar("wwwroot/mount/import/"); set => SetPar(value); }
        public string ImageExportFolder { get => GetPar("wwwroot/mount/album/"); set => SetPar(value); }
        public string ImageRecycleFolder { get => GetPar("wwwroot/mount/recycle/"); set => SetPar(value); }
        public string ImageThumbnailFolder { get => GetPar("wwwroot/mount/thumbnails/"); set => SetPar(value); }
        public Size ImageThumbnailSize { get => GetPar(new Size(0, 512)); set => SetPar(value); }
        public bool ImageUseRecycleFolder { get => GetPar(true); set => SetPar(value); }
        public ImageQualityCompareMethods ImageQualityCompareMethod { get => GetPar(ImageQualityCompareMethods.Resolution); set => SetPar(value); }
        public ImageHashingAlgorithms ImageHashingAlgorithm { get => GetPar(ImageHashingAlgorithms.AHashing); set => SetPar(value); }
    }






}
