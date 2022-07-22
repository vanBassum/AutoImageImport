namespace ImageImporter.Application.Hashing
{
    public interface IHashGenerator
    {
        Task<ulong> Generate(string filename);
    }


}
