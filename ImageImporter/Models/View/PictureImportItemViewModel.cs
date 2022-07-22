

using ImageImporter.Data;
using ImageImporter.Models.Db.ActionItems;
using ImageImporter.Models.Enums;

namespace ImageImporter.Models.View
{
    public class PictureImportItemViewModel
    {
        public int Id { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public string? File { get; set; }
        public string? Hash { get; set; }
        public string? Thumbnail { get; set; }
        public bool? KeptSource { get; set; }
        public string? RemovedFile { get; set; }
        public string? RemovedFileThumbnail { get; set; }
        public ActionType ActionType { get; set; }


    }

}
