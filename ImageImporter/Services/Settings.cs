using ImageImporter.Data;
using ImageImporter.Models.Enums;

namespace ImageImporter.Services
{
    public class Settings : SettingsService
    {
        public Settings(ApplicationDbContext context) : base(context)
        {
        }

        public string ImageImportFolder { get => GetPar("wwwroot/mount/import/"); set => SetPar(value); }
        public string ImageExportFolder { get => GetPar("wwwroot/mount/export/"); set => SetPar(value); }
        public string ImageRecycleFolder { get => GetPar("wwwroot/mount/recycle/"); set => SetPar(value); }

        public bool ImageUseRecycleFolder { get => GetPar(true); set => SetPar(value); }
        public int ImageHashWidth { get => GetPar(32); set => SetPar(value); }
        public int ImageHashHeight { get => GetPar(32); set => SetPar(value); }
        public ImageQualityCompareMethods ImageQualityCompareMethod { get => GetPar(ImageQualityCompareMethods.Resolution); set => SetPar(value); }
        public ImageHashingAlgorithms ImageHashingAlgorithm { get => GetPar(ImageHashingAlgorithms.AHashing); set => SetPar(value); }
    }





}
