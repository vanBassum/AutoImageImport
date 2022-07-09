namespace ImageImporter.Application.Importers
{
    public class ImportResult
    {
        public bool Success { get; set; } = false;
        public ImportStatus Status { get; set; } = ImportStatus.Unknown;
        public Exception? Exception { get; set; }
    }


    public enum ImportStatus
    { 
        Unknown = 0,
        IncorrectFileType = 1,
        HashingAlgorithmUndetermined = 2,
        ImportedUniqueFile = 3,
        ExceptionThrown = 4,
        ImportedUniqueFileWithRename = 5,
        ExactDuplicateDeletedSource = 6,
        MatchingDuplicateDeletedSource = 7,
        MatchingDuplicateRecycledSource = 8,
        MatchingDuplicateRecycledAndRenamedSource = 9,
        MatchingDuplicateKeptSource = 10,
    }



}
