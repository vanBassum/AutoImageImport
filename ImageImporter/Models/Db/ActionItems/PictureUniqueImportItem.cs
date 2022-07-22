﻿namespace ImageImporter.Models.Db.ActionItems
{
    public class PictureUniqueImportItem : ActionItem
    {
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public virtual Picture? Picture { get; set; }
    }
}