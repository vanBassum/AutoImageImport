namespace ImageImporter.Application.Importers
{
    public class ImportResult
    {
        public bool Success { get; set; } = false;
        public ImportError Error { get; set; } = ImportError.Unknown;

    }


    public enum ImportError
    { 
        Unknown = 0,
        IncorrectFileType = 1,
        HashingAlgorithmUndetermined = 2,
    }



}
