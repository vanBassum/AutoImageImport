using ImageImporter.Models;

namespace ImageImporter.Application.Commands
{
    public class Import : BaseCommand
    {
        private AppSettings AppSettings { get; }
        private AppDBContext AppDBContext { get; }

        public Import(AppSettings appSettings, AppDBContext appDBContext)
        {
            AppSettings = appSettings;
            AppDBContext = appDBContext;
        }

        public override void Execute(string args)
        {
            foreach (string file in Directory.GetFiles(AppSettings.ImportFolder, "", SearchOption.AllDirectories))
            {
                Picture? picture = Picture.Generate(AppSettings, file);
                if(picture?.RelativePath != null)
                {
                    Picture? matchingPicture = AppDBContext.Pictures.FirstOrDefault(a => a.HashA == picture.HashA);
                    if (matchingPicture != null)
                    {
                        Engine.Log(
                            $"Not imported, duplicate hash. \r\n" +
                            $"  {picture}\r\n" +
                            $"  {matchingPicture}");
                    }
                    else
                    {
                        string savePath = Path.Combine(AppSettings.ImagesFolder, picture.RelativePath);

                        if(File.Exists(savePath))
                        {
                            Engine.Log(
                                $"Not imported, duplicate filename. \r\n" +
                                $"  {picture}");
                        }
                        else
                        {
                            string? directory = Path.GetDirectoryName(savePath);
                            if(directory != null)
                            {
                                Directory.CreateDirectory(directory);
                                File.Move(file, savePath);
                                Engine.Log(
                                    $"Imported. \r\n" +
                                    $"  {picture}");

                                AppDBContext.Add(picture);
                                AppDBContext.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    string relativeFile = Path.GetRelativePath(AppSettings.ImportFolder, file);
                    Engine.Log(
                        $"Not imported, not an image. \r\n" +
                        $"  Filename = '{relativeFile}'");
                }
            }
        }
    }


    public class Hamming : BaseCommand
    {
        private AppSettings AppSettings { get; }
        private AppDBContext AppDBContext { get; }

        public Hamming(AppSettings appSettings, AppDBContext appDBContext)
        {
            AppSettings = appSettings;
            AppDBContext = appDBContext;
        }

        public override void Execute(string args)
        {
            List<(double Match, string Message)> matches = new();

            foreach(var pictureA in AppDBContext.Pictures.OrderBy(a => a.Id).ToList())
            {
                foreach(var pictureB in AppDBContext.Pictures.Where(a=>a.Id > pictureA.Id).ToList())
                {
                    double match = 1 - HammingDistance(pictureA.HashA, pictureB.HashA);

                    if(match > 0.75)
                    {
                        matches.Add((match,
                            $"{match.ToString("P")}\r\n" +
                            $"  {pictureA.RelativePath}\r\n" +
                            $"  {pictureB.RelativePath}"));
                    }
                }
            }

            foreach(var match in matches.OrderBy(a=>a.Match))
            {
                Engine.Log(match.Message);
            }


        }

        public double HammingDistance(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                throw new Exception();
            double distance = 0;
            for (int i = 0; i < a.Length; i++)
            {
                for (int bit = 0; bit < 8; bit++)
                {
                    byte mask = (byte)(1 << bit);
                    if ((a[i] & mask) != (b[i] & mask))
                        distance++;
                }

                //if (a[i] != b[i])
                //    distance++;    
            }
            return distance /= a.Length * 8;
        }
    }
}
