namespace ImageImporter.Application.Comparers
{
    public interface IQualityComparer
    {
        Task<bool> CheckIfSourceIsBetter(string source, string destination);
    }


}
