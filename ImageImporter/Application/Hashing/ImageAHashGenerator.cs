using CoenM.ImageHash.HashAlgorithms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageImporter.Application.Hashing
{
    public class ImageAHashGenerator : IHashGenerator
    {
        public async Task<byte[]?> Generate(string filename)
        {
            return await Task.Run(() => GenerateSync(filename));
        }

        byte[] GenerateSync(string filename)
        {
            var hashAlgorithm = new AverageHash();
            using var image = Image.Load<Rgba32>(filename);
            return BitConverter.GetBytes(hashAlgorithm.Hash(image));
        }
    }
}
