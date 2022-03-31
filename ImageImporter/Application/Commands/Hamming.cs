namespace ImageImporter.Application.Commands
{
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

            var pictures = AppDBContext.Pictures.OrderBy(a => a.Id).ToList();


            foreach (var pictureA in pictures)
            {
                foreach(var pictureB in pictures.Where(a=>a.Id > pictureA.Id))
                {
                    double? match = 1 - HammingDistance(pictureA.HashA, pictureB.HashA);

                    if(match != null)
                    {
                        if (match > 0.75)
                        {
                            matches.Add((match.Value,
                                $"{match?.ToString("P")}\r\n" +
                                $"  {pictureA.RelativePath}\r\n" +
                                $"  {pictureB.RelativePath}"));
                        }
                    }
                }
            }

            foreach(var match in matches.OrderBy(a=>a.Match))
            {
                Engine.Log(match.Message);
            }


        }

        public double? HammingDistance(byte[]? a, byte[]? b)
        {
            if(a!=null && b!=null)
            {
                if(a.Length == b.Length)
                {
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
            return null;
        }
    }
}
