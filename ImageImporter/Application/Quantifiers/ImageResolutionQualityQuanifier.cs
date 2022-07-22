using SixLabors.ImageSharp;

namespace ImageImporter.Application.Quantifiers
{
    public class ImageResolutionQualityQuanifier : IQualityQuantifier
    {
        public async Task<int> DetermineQuality(string file)
        {
            IImageInfo srcInfo = Image.Identify(file);
            return (srcInfo.Height * srcInfo.Width);
        }

    }


}
