namespace ImageImporter.Models.Enums
{
    public enum ImportStatus
    {
        Unknown = 0,
        IncorrectFileType = 1,
        HashingFailed = 2,
        ExceptionThrown = 3,
        CoulntDetermineBestQuality = 4,
        ImportedUniqueFile = 10,
        HashMatchKeptSource = 11,
        HashMatchDeletedSource = 12,
    }
}
