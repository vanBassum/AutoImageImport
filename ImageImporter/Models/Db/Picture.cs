using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageImporter.Models.Db
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public string? Path { get; set; }
        public byte[]? Hash { get; set; }
        [Column(TypeName = "text")]
        public string? Thumbnail { get; set; }

    }

}
