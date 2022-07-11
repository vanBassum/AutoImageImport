using ImageImporter.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class ImportResult
    {
        [Key]
        public int Id { get; set; }
        public bool Success { get; set; } = false;
        public ImportStatus Status { get; set; } = ImportStatus.Unknown;
        public string? Exception { get; set; }
        public string? RelativePath { get; set; }
        public byte[]? Hash { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? JobStartTime { get; set; }

        public ImportResult()
        {
            Success = false;
            Status = ImportStatus.Unknown;
            Timestamp = DateTime.Now;
        }

        public void SetResult(bool success, ImportStatus status)
        {
            Success = success;
            Status = status;
        }

        public void SetException(bool success, Exception exception)
        {
            Success = success;
            Status = ImportStatus.ExceptionThrown;
            Exception = exception.Message;
        }
    }
}
