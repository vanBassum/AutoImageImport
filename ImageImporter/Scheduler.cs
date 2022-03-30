using ImageImporter.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

// See https://aka.ms/new-console-template for more information

public class Scheduler
{
    private AppDBContext AppDBContext { get; set; }
    private AppSettings AppSettings { get; set; }
    public Scheduler(AppDBContext appDBContext, AppSettings appSettings)
    {
        AppDBContext = appDBContext;
        AppSettings = appSettings;
    }


    public int HammingDistance(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            throw new Exception();
        int distance = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
                distance++;
        }
        return distance;
    }


    public void Execute()
    {
        ImageProcessor processor = new ImageProcessor();
        foreach (string file in Directory.GetFiles(AppSettings.ImportFolder))
        {
            string relativeFile = Path.GetRelativePath(AppSettings.ImportFolder, file);
            if (!AppDBContext.Pictures.Any(a => a.RelativePath == relativeFile))
            {
                byte[] aHash = processor.CalculateAverageHashWithColor(file);
                //if (!AppDBContext.Pictures.Any(a => a.AHash == aHash))
                {
                    Picture pic = new Picture();
                    pic.AHash = aHash;
                    pic.RelativePath = relativeFile;
                    AppDBContext.Add(pic);
                }
            }
        }
        AppDBContext.SaveChanges();


        List<(double percentage, Picture picA, Picture picB)> matches = new List<(double, Picture, Picture)>();
        var pictures = AppDBContext.Pictures.ToList();
        for (int i = 0; i < pictures.Count; i++)
        {
            for (int p = i + 1; p < pictures.Count; p++)
            {
                int distance = HammingDistance(pictures[i].AHash, pictures[p].AHash);

                double match = 1-((double)distance / (double)pictures[i].AHash.Length);

                if(match > 0.05)
                {
                    matches.Add((match, pictures[i], pictures[p]));
                    
                }
            }
        }

        foreach(var match in matches.OrderBy(a=>a.percentage))
        {
            Console.WriteLine($"{match.percentage.ToString("P")}, {match.picA.RelativePath} | {match.picB.RelativePath}");
        }

    }
}



public class ImageProcessor
{
    public int HashWidth { get; set; } = 16;
    public int HashHeight { get; set; } = 16;

    //http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html
    public byte[] CalculateAverageHash(string filename)
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



    public byte[] CalculateAverageHashWithColor(string filename)
    {
        List<byte> data = new List<byte>();

        using (var image = Image.Load<Rgba32>(filename))
        {
            image.Mutate(x => x
                        .AutoOrient()
                        .Resize(HashWidth, HashHeight));

            long meanR = 0;
            long meanG = 0;
            long meanB = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    meanR += image[x, y].R;
                    meanG += image[x, y].G;
                    meanB += image[x, y].B;
                }
            }


            meanR /= HashWidth * HashHeight;
            meanG /= HashWidth * HashHeight;
            meanB /= HashWidth * HashHeight;



            int i = 0;
            byte bR = 0;
            byte bG = 0;
            byte bB = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (i == 0)
                        bR = bG = bB = 0;

                    var pixel = image[x, y];

                    if(pixel.R >meanR)
                        bR |= (byte)(0x80 >> i);

                    if (pixel.G > meanG)
                        bG |= (byte)(0x80 >> i);

                    if (pixel.B > meanB)
                        bB |= (byte)(0x80 >> i);

                    i++;
                    if (i == 8)
                    {
                        data.Add(bR);
                        data.Add(bG);
                        data.Add(bB);
                        i = 0;
                    }
                }
            }
        }

        return data.ToArray();
    }
}
