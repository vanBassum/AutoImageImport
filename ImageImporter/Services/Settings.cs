using ImageImporter.Data;

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

        public bool ImageRecycleMatches { get => GetPar(true); set => SetPar(value); }
        public int ImageHashWidth { get => GetPar(32); set => SetPar(value); }
        public int ImageHashHeight { get => GetPar(32); set => SetPar(value); }
        public ImageHashingAlgorithms ImageHashingAlgorithm { get => GetPar(ImageHashingAlgorithms.DHashing); set => SetPar(value); }
    }


    public enum ImageHashingAlgorithms
    {
        AHashing = 0,
        DHashing = 1,
    }



}
