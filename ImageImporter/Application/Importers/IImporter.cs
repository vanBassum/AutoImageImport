namespace ImageImporter.Application.Importers
{
    public interface IImporter
    {
        Task<ImportResult> ImportFile(string file);
    }

}
