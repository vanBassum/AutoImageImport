using CoenM.ImageHash.HashAlgorithms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageImporter.Application.Hashing
{
    public class ImageDHashGenerator : IHashGenerator
    {
        public async Task<ulong> Generate(string filename)
        {
            return await Task.Run(() => GenerateSync(filename));
        }

        ulong GenerateSync(string filename)
        {
            var hashAlgorithm = new DifferenceHash();
            using var image = Image.Load<Rgba32>(filename);
            return hashAlgorithm.Hash(image);
        }
    }
}
