using ImageImporter.Data;
using ImageImporter.Models.Db;
using ImageImporter.Models.Enums;
using ImageImporter.Services;

namespace ImageImporter.Application.Importers
{
    public class FileImporter : IImporter
    {
        private Settings Settings { get; }
        private ApplicationDbContext Context { get; }
        private ImageImporter ImageImporter { get; }

        public FileImporter(Settings settings, ApplicationDbContext context)
        {
            Settings = settings;
            Context = context;
            ImageImporter = new ImageImporter(settings, context);
        }


        public async Task ImportFile(string file, ImportResult result)
        {
            result.ImporterType = ImporterTypes.ImageImporter;
            await ImageImporter.ImportFile(file, result);



            //TODO: Other importers!
        }
    }

}
