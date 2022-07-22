namespace ImageImporter.Models.Db.ActionItems
{
    public class PictureMatchImportItem : PictureImportItem
    {
        public bool? KeptSource { get; set; }
        public string? RemovedFile { get; set; }
        public string? RemovedFileThumbnail { get; set; }

    }

}
