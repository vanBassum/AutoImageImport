using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public DbFile? File { get; set; }
        public byte[]? Hash { get; set; }
        public string? Thumbnail { get; set; }
    }
}
