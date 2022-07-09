using ImageImporter.Application.Hashing;
using ImageImporter.Data;
using ImageImporter.Services;
using SixLabors.ImageSharp;

namespace ImageImporter.Application.Importers
{
    public class ImageImporter : IImporter
    {
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        public ImageImporter(Settings settings, ApplicationDbContext context)
        {
            Settings = settings;
            Context = context;
        }

        public async Task<ImportResult> ImportFile(FileInfo file)
        {
            ImportResult result = new ImportResult();
            IImageInfo imageInfo = Image.Identify(file.FullName);

            if (imageInfo != null)
            {
                IImageHashGenerator? hashGenerator = null;

                switch(Settings.ImageHashingAlgorithm)
                {
                    case ImageHashingAlgorithms.AHashing:
                        hashGenerator = new ImageAHashGenerator();
                        break;
                    case ImageHashingAlgorithms.DHashing:
                        hashGenerator= new ImageDHashGenerator();
                        break;   
                }
                
                if(hashGenerator!=null)
                {
                    hashGenerator.HashWidth = Settings.ImageHashWidth;
                    hashGenerator.HashHeight = Settings.ImageHashHeight;
                    var hash = hashGenerator.Generate(file.FullName);

                    // TODO:



                }
                else
                {
                    result.Error = ImportError.HashingAlgorithmUndetermined;
                    result.Success = false;
                }
            }
            else
            {
                result.Error = ImportError.IncorrectFileType;
                result.Success = false;
            }


            return result;
        }
    }

}
