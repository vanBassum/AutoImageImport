using ImageImporter.Models.Db.ActionItems;
using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db
{
    public class JobResult
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<ActionItem> ActionsLog { get; set; } = new List<ActionItem>();
        public DateTime? Started { get; set; }
        public TimeSpan? Duration { get; set; } 
    }








}

