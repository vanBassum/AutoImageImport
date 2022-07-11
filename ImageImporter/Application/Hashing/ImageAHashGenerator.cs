using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageImporter.Application.Hashing
{
    public class ImageAHashGenerator : IHashGenerator
    {
        public int HashWidth { get; set; } = 32;
        public int HashHeight { get; set; } = 32;

        public async Task<byte[]> Generate(string filename)
        {
            return await Task.Run(() => GenerateSync(filename));
        }

        byte[] GenerateSync(string filename)
        {
            List<byte> data = new List<byte>();

            using (var image = Image.Load<Rgba32>(filename))
            {
                image.Mutate(x => x
                            .AutoOrient()
                            .Resize(HashWidth, HashHeight)
                            .Grayscale());

                long mean = 0;
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        mean += image[x, y].R;
                    }
                }
                mean /= image.Height * image.Width;
                int i = 0;
                byte b = 0;
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        if (i == 0)
                            b = 0;

                        byte pxVal = image[x, y].R;
                        if (pxVal > mean)
                        {
                            b |= (byte)(0x80 >> i);
                        }
                        i++;
                        if (i == 8)
                        {
                            data.Add(b);
                            i = 0;
                        }
                    }
                }
            }
            return data.ToArray();
        }
    }


}
