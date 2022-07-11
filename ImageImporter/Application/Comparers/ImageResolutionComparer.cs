using SixLabors.ImageSharp;

namespace ImageImporter.Application.Comparers
{
    public class ImageResolutionComparer : IQualityComparer
    {
        public async Task<bool> CheckIfSourceIsBetter(string source, string destination)
        {
            IImageInfo srcInfo = Image.Identify(source);
            IImageInfo dstInfo = Image.Identify(destination);


            return (srcInfo.Height * srcInfo.Width) > (dstInfo.Height * dstInfo.Width);
        }

    }

}
