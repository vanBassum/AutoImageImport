namespace ImageImporter.Application.Comparers
{
    public class FileSizeComparer : IQualityComparer
    {
        public async Task<bool> CheckIfSourceIsBetter(string source, string destination)
        {
            var src = new FileInfo(source);
            var dst = new FileInfo(destination);

            return src.Length > dst.Length;
        }

    }

}
