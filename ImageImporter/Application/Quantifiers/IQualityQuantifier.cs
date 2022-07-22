namespace ImageImporter.Application.Quantifiers
{
    public interface IQualityQuantifier
    {
        Task<int> DetermineQuality(string file);
    }


}
