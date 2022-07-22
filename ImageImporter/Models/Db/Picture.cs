using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{

    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public string? File { get; set; }
        public long? Hash { get; set; }
        public string? Thumbnail { get; set; }
        public int Quality { get; set; }
        public bool Deleted { get; set; } = false;
        public int Width { get; set; }
        public int Height { get; set; }
        public long FileSize { get; set; }


    }
}
