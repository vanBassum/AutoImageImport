using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageImporter.Models
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(32)]
        public byte[] HashA { get; set; }
        [MaxLength(32)]
        public byte[] HashD { get; set; }
        public string? RelativePath { get; set; }
    }
}
