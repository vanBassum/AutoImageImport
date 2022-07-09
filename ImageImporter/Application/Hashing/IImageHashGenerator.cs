namespace ImageImporter.Application.Hashing
{
    public interface IImageHashGenerator
    {
        int HashWidth { get; set; }
        int HashHeight { get; set; }
        Task<byte[]> Generate(string filename);
    }


}
