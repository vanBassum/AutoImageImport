using ImageImporter.Data;
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


        public async Task<ImportResult> ImportFile(string file)
        {
            ImportResult result = new ImportResult();

            if(!result.Success)
                result = await ImageImporter.ImportFile(file);


            return result;
        }
    }

}
