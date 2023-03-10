using System.ComponentModel.DataAnnotations;

namespace MediaImporter.Models
{
    public class WebFile
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
    }

}
