using ImageImporter.Models.Db;

namespace ImageImporter.Application.Importers
{
    public interface IImporter
    {
        Task ImportFile(string file, ImportResult result);
    }

}
