using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageImporter.Application
{
    public class ImageDHashGenerator : IImageHashGenerator
    {
        public int HashWidth { get; set; } = 32;
        public int HashHeight { get; set; } = 32;
        public byte[] Generate(string filename)
        {
            List<byte> data = new List<byte>();

            using (var image = Image.Load<Rgba32>(filename))
            {
                image.Mutate(x => x
                            .AutoOrient()
                            .Resize(HashWidth + 1, HashHeight)
                            .Grayscale());
                int i = 0;
                byte b = 0;
                for (int y = 0; y < image.Height; y++)
                {
                    byte prevPx = image[0, y].R;
                    for (int x = 1; x < image.Width; x++)
                    {
                        if (i == 0)
                            b = 0;

                        byte pxVal = image[x, y].R;
                        if (pxVal > prevPx)
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