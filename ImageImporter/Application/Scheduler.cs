using ImageImporter.Application;
using ImageImporter.Models;
using System.Diagnostics;

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



