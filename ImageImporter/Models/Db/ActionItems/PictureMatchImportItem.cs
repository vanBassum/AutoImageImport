namespace ImageImporter.Models.Db.ActionItems
{
    public class PictureMatchImportItem : ActionItem
    {
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public bool? KeptSource { get; set; }
        public string? RemovedFile { get; set; }
        public string? RemovedFileThumbnail { get; set; }
        public virtual Picture? Picture { get; set; }
    }
}
