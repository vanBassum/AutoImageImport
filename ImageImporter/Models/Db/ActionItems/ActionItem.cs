using System.ComponentModel.DataAnnotations;

namespace ImageImporter.Models.Db.ActionItems
{


    public class ActionItem
    {
        [Key]
        public int Id { get; set; }
        public virtual List<ActionItem> ActionsLog { get; set; } = new List<ActionItem>();
    }

}
