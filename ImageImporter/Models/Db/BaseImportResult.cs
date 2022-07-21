using ImageImporter.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class BaseImportResult
    {
        [Key]
        public int Id { get; set; }
        public bool Success { get; set; } = false;
        public ImportStatus Status { get; set; } = ImportStatus.Unknown;
        public DateTime? Timestamp { get; set; }
        public DateTime? JobStartTime { get; set; }
        public string? Exception { get; set; }
        public byte[]? Hash { get; set; }

        public virtual DbFile? RemovedFile { get; set; }

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
