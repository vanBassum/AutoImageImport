namespace ImageImporter.Models.Db.ActionItems
{
    public class PictureRemoveItem : ActionItem
    {
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public string? Thumbnail { get; set; }
    }
}
