using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public string? Path { get; set; }
        public byte[]? Hash { get; set; }


    }

}
