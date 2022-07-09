using ImageImporter.Data;

namespace ImageImporter.Services
{
    public class Settings : SettingsService
    {
        public Settings(ApplicationDbContext context) : base(context)
        {
        }

        public string ImportFolder { get => GetPar("wwwroot/mount/import/"); set => SetPar(value); }

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
