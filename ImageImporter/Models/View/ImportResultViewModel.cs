using ImageImporter.Data;
using ImageImporter.Models.Db;

namespace ImageImporter.Models.View
{
    public class ImportResultViewModel
    {
        public int Id { get; set; }
        public bool Success { get; set; } = false;
        public string Status { get; set; }
        public string Exception { get; set; }
        public string RelativePath { get; set; }
        public string Hash { get; set; }
        public string Timestamp { get; set; }
        public string JobStartTime { get; set; }
        public string? Thumb { get; set; }
        public string? RemovedThumb { get; set; }

        public ImportResultViewModel(ImportResult result)
        {
            Id = result.Id;
            Success = result.Success;    
            Status = result.Status.ToString();
            Exception = result.Exception;
            RelativePath = result.RelativePath;
            Hash = result.Hash!=null?Convert.ToHexString(result.Hash):"";
            Timestamp = result.Timestamp?.ToString(@"dd-MM-yyyy HH:mm:ss");
            JobStartTime = result.JobStartTime?.ToString(@"dd-MM-yyyy HH:mm:ss");
            RemovedThumb = result.RemovedFileThumb;
        }

    }
}
