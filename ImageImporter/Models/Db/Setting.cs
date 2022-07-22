using ImageImporter.Data;
using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Value { get; set; }

    }








}

