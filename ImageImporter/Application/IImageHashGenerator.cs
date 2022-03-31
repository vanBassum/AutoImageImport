
namespace ImageImporter.Application
{

    public interface IImageHashGenerator
    {
        int HashWidth { get; set; }
        int HashHeight { get; set; }
        byte[] Generate(string filename);
    }




}