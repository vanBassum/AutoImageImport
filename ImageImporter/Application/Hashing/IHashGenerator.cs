namespace ImageImporter.Application.Hashing
{
    public interface IHashGenerator
    {
        Task<byte[]> Generate(string filename);
    }


}
