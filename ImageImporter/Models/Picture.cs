using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageImporter.Models
{
    [Index(nameof(Id), nameof(AHash))]
    public class Picture
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1024)]
        public byte[]? AHash { get; set; }
        public string? RelativePath { get; set; }
    }
}
