using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class DbFile
    {
        [Key]
        public int Id { get; set; }
        public string? Path { get; set; }






    }

}
