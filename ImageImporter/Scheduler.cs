using ImageImporter.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

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


    public double HammingDistance(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            throw new Exception();
        double distance = 0;
        for (int i = 0; i < a.Length; i++)
        {
            for(int bit=0; bit<8; bit++)
            {
                byte mask = (byte)(1<<bit);
                if((a[i] & mask)!= (b[i] & mask))
                    distance++;
            }

            //if (a[i] != b[i])
            //    distance++;    
        }
        return distance /= a.Length * 8;
    }


    public void Execute()
    {
        IImageHashGenerator aHashGenerator = new ImageAHashGenerator();
        IImageHashGenerator dHashGenerator = new ImageDHashGenerator();

        foreach (string file in Directory.GetFiles(AppSettings.ImportFolder))
        {
            string relativeFile = Path.GetRelativePath(AppSettings.ImportFolder, file);
            if (!AppDBContext.Pictures.Any(a => a.RelativePath == relativeFile))
            {
                Stopwatch stopwatch1 = Stopwatch.StartNew();
                var aHash = aHashGenerator.Generate(file);
                stopwatch1.Stop();
                Stopwatch stopwatch2 = Stopwatch.StartNew();
                var dHash = dHashGenerator.Generate(file);
                stopwatch2.Stop();
                //if (!AppDBContext.Pictures.Any(a => a.AHash == aHash))
                {
                    Picture pic = new Picture();
                    pic.HashA = aHash;
                    pic.HashD = dHash;
                    pic.RelativePath = relativeFile;
                    AppDBContext.Add(pic);
                }
            }
        }
        AppDBContext.SaveChanges();




        List<(string method, double percentage, Picture picA, Picture picB)> matches = new List<(string, double, Picture, Picture)>();
        var pictures = AppDBContext.Pictures.ToList();
        for (int i = 0; i < pictures.Count; i++)
        {
            for (int p = i + 1; p < pictures.Count; p++)
            {
                double distance = HammingDistance(pictures[i].HashA, pictures[p].HashA);
                double match = (double)1 - distance;

                if(match > 0.0)
                {
                    matches.Add(("HashA", match, pictures[i], pictures[p]));
                }
            }
        }

        for (int i = 0; i < pictures.Count; i++)
        {
            for (int p = i + 1; p < pictures.Count; p++)
            {
                double distance = HammingDistance(pictures[i].HashD, pictures[p].HashD);
                double match = (double)1 - distance;
                if (match > 0.0)
                {
                    matches.Add(("HashD", match, pictures[i], pictures[p]));
                }
            }
        }

        foreach (var match in matches.OrderBy(a=>a.percentage))
        {
            Console.WriteLine($"{match.method} {match.percentage.ToString("P")}, {match.picA.RelativePath} | {match.picB.RelativePath}");
        }

    }
}







public interface IImageHashGenerator
{
    int HashWidth { get; set; } 
    int HashHeight { get; set; }
    byte[] Generate(string filename);
}




public class ImageDHashGenerator : IImageHashGenerator
{
    public int HashWidth { get; set; } = 16;
    public int HashHeight { get; set; } = 16;
    public byte[] Generate(string filename)
    {
        List<byte> data = new List<byte>();

        using (var image = Image.Load<Rgba32>(filename))
        {
            image.Mutate(x => x
                        .AutoOrient()
                        .Resize(HashWidth+1, HashHeight)
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




public class ImageAHashGenerator : IImageHashGenerator
{
    public int HashWidth { get; set; } = 16;
    public int HashHeight { get; set; } = 16;
    public byte[] Generate(string filename)
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


