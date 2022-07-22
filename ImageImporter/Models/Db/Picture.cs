﻿using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public string? File { get; set; }
        public ulong? Hash { get; set; }
        public string? Thumbnail { get; set; }
    }
}
